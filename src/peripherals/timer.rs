use crate::peripherals::MMIO_BASE;
use register::{mmio::*, register_bitfields};

register_bitfields! {
    u32,

    CS [
        M0 OFFSET(0) NUMBITS(1) [
            NO_MATCH = 0,
            MATCH = 1
        ],
        M1 OFFSET(1) NUMBITS(1) [
            NO_MATCH = 0,
            MATCH = 1
        ],
        M2 OFFSET(2) NUMBITS(1) [
            NO_MATCH = 0,
            MATCH = 1
        ],
        M3 OFFSET(3) NUMBITS(1) [
            NO_MATCH = 0,
            MATCH = 1
        ]
    ]
}

#[allow(non_snake_case)]
#[repr(C)]
// Ideally CLO and CHI would be treated as a single register, but it would be
// unaligned so we can't read 64 bits from it (the program faults)
pub struct SystemTimer {
    CS: ReadWrite<u32, CS::Register>, // 0x00
    CLO: ReadOnly<u32>,               // 0x04
    CHI: ReadOnly<u32>,               // 0x04
    C0: ReadWrite<u32>,               // 0x0C
    C1: ReadWrite<u32>,               // 0x10
    C2: ReadWrite<u32>,               // 0x14
    C3: ReadWrite<u32>,               // 0x18
}

impl SystemTimer {
    pub fn sleep_usec(&self, duration_us: u64) {
        // TODO: Once interrupts are set up, use interrupts to set wakeup
        let initial = self.read_timer();
        // u64 will last for 10^19 years, so if it overflows then it will never
        // be hit anyway
        let target = initial.saturating_add(duration_us);
        while self.read_timer() < target {
            // noop
        }
    }

    // Get the current value of the timer (which is incremented once per microsecond)
    pub fn read_timer(&self) -> u64 {
        let mut high = self.CHI.get();
        let mut low = self.CLO.get();

        let new_high = self.CHI.get();
        if new_high != high {
            high = new_high;
            low = self.CLO.get();
            // High is incremented less than once per hour, so we can safely
            // assume that it hasn't overflowed again
        }

        (u64::from(high) << 32) + u64::from(low)
    }
}

pub fn get_timer() -> &'static SystemTimer {
    unsafe {
        &*((MMIO_BASE + 0x0000_3000) as *const SystemTimer)
    }
}

#[inline]
pub fn sleep_cycles(cycles: u64) {
    for _ in 0..cycles {
        unsafe {
            asm!("nop" :::: "volatile");
        }
    }
}
