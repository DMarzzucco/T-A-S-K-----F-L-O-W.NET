use crate::peripherals::uart0::Uart;
use crate::peripherals::mailbox;
use core::ptr;
use core::sync::atomic::{compiler_fence, Ordering};
use core::mem::transmute;

extern "C" {
    static __self_update_code_start: usize;
    static __self_update_code_end: usize;
    static __program_end: usize;
}

#[derive(Debug)]
pub enum UpdateError {
    MailboxError(mailbox::MailboxError),
    SizeError,
}

#[cfg(target_arch = "aarch64")]
pub fn self_update(uart:  &Uart) -> Result<!, UpdateError> {
    let self_update_code_start: usize = unsafe { &__self_update_code_start as *const usize as usize };
    let self_update_code_end: usize = unsafe { &__self_update_code_end as *const usize as usize };
    let program_end: usize = unsafe { &__program_end as *const usize as usize };

    // To get here, the host should have notified us that it wants to update
    // It will now be sending the size
    let mut new_size: u32 = uart.getc() as u8 as u32;
    new_size |= (uart.getc() as u8 as u32) << 8;
    new_size |= (uart.getc() as u8 as u32) << 16;
    new_size |= (uart.getc() as u8 as u32) << 24;

    let new_size = new_size as usize;

    // Query the GPU to find out how much RAM we have
    let (_base, available_memory) = match mailbox::get_memory_range() {
        Ok(r) => r,
        Err(e) => {
            uart.send(0x18 as char);
            return Err(UpdateError::MailboxError(e))
        }
    };

    let self_update_code_len = self_update_code_end - self_update_code_start;

    if new_size > available_memory as usize - 0x80_000 - self_update_code_len {
        // Not enough RAM
        uart.send(0x18 as char);
        return Err(UpdateError::SizeError);
    }

    unsafe {
        // We have space for it, so now we need to relocate the self update assembly
        let new_self_update_loc = max(new_size + 0x80_000, program_end);

        ptr::copy_nonoverlapping(self_update_code_start as *const u8, new_self_update_loc as *mut u8, self_update_code_len);

        compiler_fence(Ordering::SeqCst);

        // Clear the instruction cache and flush the pipeline
        asm!("IC IALLU
              ISB");

        // We're ready to receive it - let the host know
        uart.send(0x12 as char);

        // Now construct a pointer to the new function
        let self_update_fn: extern "C" fn (usize, usize, &Uart) -> ! = transmute(new_self_update_loc);
        // (the signature is (start_address, length, uart_addr))
        // finally, call it
        self_update_fn(0x80_000, new_size as usize, uart)
    }
}

#[cfg(not(target_arch = "aarch64"))]
pub fn self_update(uart:  &Uart) -> Result<!, UpdateError> {
    unimplemented!();
}

fn max(a: usize, b: usize) -> usize {
    if a > b {
        a
    } else {
        b
    }
}

#[cfg(target_arch = "aarch64")]
global_asm!(include_str!("self_update.S"));