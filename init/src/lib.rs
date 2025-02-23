#![no_std]
#![feature(global_asm)]

use core::ptr;

// Wrap the user-defined entry point
#[macro_export]
macro_rules! entry {
    ($path:path) => {
        #[export_name = "main"]
        pub unsafe fn __main() -> ! {
            // type check the given path
            let f: fn() -> ! = $path;

            f()
        }
    };
}

#[no_mangle]
pub unsafe extern "C" fn reset() -> ! {
    extern "C" {
        // The linker will assign the addresses of these variables to be the
        // start and end of the relevant sections (so &__bss_start = &.bss)

        // .bss section
        static mut __bss_start: u64;
        static mut __bss_end: u64;

        // .data section
        static mut __data_start: u64;
        static mut __data_end: u64;
        static __data_static: u64; // Memory mapped address of the static variables
    }

    // .bss contains the zero-initialised static variables, so we need to zero
    // that section of the memory
    let mut addr: *mut u64 = &mut __bss_start;
    while (addr as usize) < (&mut __bss_end as *mut u64 as usize) {
        ptr::write_volatile(addr, 0);
        addr = addr.offset(1);
    }

    // .data contains non-zero static variables, so we need to copy it from the
    // binary
    let count = (&__data_end as *const u64 as usize) - (&__data_start as *const u64 as usize);
    ptr::copy_nonoverlapping(&__data_static, &mut __data_start, count);

    extern "Rust" {
        fn main() -> !;
    }

    main();
}

#[cfg(target_arch = "aarch64")]
global_asm!(include_str!("boot_cores.S"));