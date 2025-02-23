use core::cell::UnsafeCell;
use core::hint::spin_loop;
use core::ops::{Deref, DerefMut};
use core::sync::atomic::{AtomicBool, Ordering, AtomicU64};

/// A test-and-test-and-set lock
pub struct Mutex<T> {
    lock: AtomicBool,
    data: UnsafeCell<T>,
}

impl<T> Mutex<T> {
    /// Construct a new mutex to protect a value
    pub const fn new(v: T) -> Mutex<T> {
        Mutex {
            lock: AtomicBool::new(false),
            data: UnsafeCell::new(v),
        }
    }

    /// Lock the mutex and return a wrapper around the protected result
    #[cfg(any(not(target_arch = "aarch64"), feature = "no-atomics"))]
    pub fn lock(&self) -> LockedMutex<'_, T> {
        loop {
            if !self.lock.load(Ordering::Acquire) {
                if self.try_set_lock_locked() {
                    return LockedMutex {
                        mutex: self
                    }
                }
            }
            spin_loop();
        }
    }

    #[cfg(all(not(feature = "no-atomics"), target_arch = "aarch64"))]
    pub fn lock(&self) -> LockedMutex<'_, T> {
        let l: *const AtomicBool = &self.lock as *const AtomicBool;
        unsafe {
            asm!("1:
                    // Load non-exclusive, with acquire to ensure that other effects are seen
                    LDARB   w1, [$0]
                    CBNZ    w1, 3f
                    // If the lock was 0, load it exclusively and make sure it's still 0
                    LDAXRB  w1, [$0]
                    CBZ     w1, 2f
                    // If we loaded it exclusively 
                    STXRB   w1, ${1:w}, [$0]
                    CBZ     w1, 4f // w1 indicates whether the store succeeded
                2:
                    CLREX
                3:
                    YIELD
                    B       1b
                4:
                " :: "r"(l), "r"(1) : "w1", "memory" : "volatile");
        }
        LockedMutex {
            mutex: self
        }
    }

    /// If the mutex is unlocked, lock it and return a wrapper around the
    /// protected result; otherwise return None
    pub fn try_lock(&self) -> Option<LockedMutex<'_, T>> {
        if self.try_set_lock_locked() {
            Some(LockedMutex {
                mutex: self
            })
        } else {
            None
        }
    }

    /// Unlock the mutex. This method must only be called when dropping
    /// LockedMutex
    fn unlock(&self) {
        self.lock.store(false, Ordering::Release);
    }

    #[cfg(not(feature = "no-atomics"))]
    fn try_set_lock_locked(&self) -> bool {
        !self.lock.compare_and_swap(false, true, Ordering::Acquire)
    }

    #[cfg(feature = "no-atomics")]
    fn try_set_lock_locked(&self) -> bool {
        if !self.lock.load(Ordering::Acquire) {
            self.lock.store(true, Ordering::Relaxed);
            true
        } else {
            false
        }
    }
}

unsafe impl<T: Send> Sync for Mutex<T> { }

/// The result of locking a mutex
pub struct LockedMutex<'a, T> {
    mutex: &'a Mutex<T>,
}

impl<'a, T> LockedMutex<'a, T> {
    /// Get a reference to the value protected by this mutex
    pub fn value(&self) -> &T {
        unsafe {
            &*self.mutex.data.get()
        }
    }

    /// Get a mutable reference to the value protected by this mutex
    pub fn value_mut(&mut self) -> &mut T {
        unsafe {
            &mut *self.mutex.data.get()
        }
    }
}

impl<'a, T> Drop for LockedMutex<'a, T> {
    fn drop(&mut self) {
        self.mutex.unlock();
    }
}

impl<'a, T> Deref for LockedMutex<'a, T> {
    type Target = T;

    fn deref(&self) -> &T {
        self.value()
    }
}

impl<'a, T> DerefMut for LockedMutex<'a, T> {
    fn deref_mut(&mut self) -> &mut T {
        self.value_mut()
    }
}


#[cfg(test)]
mod test {
    use super::*;

    #[test]
    pub fn repeated_acquire() {
        let m = Mutex::new(());
        drop(m.lock());
        drop(m.lock());
    }
}
