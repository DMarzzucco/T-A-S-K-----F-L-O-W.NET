use crate::peripherals::MMIO_BASE;
use register::{mmio::*, register_bitfields};
use core::hint::spin_loop;

// It starts with limited entropy, so query it a bunch of times to begin with
const WARMUP_COUNT: u32 = 0x4_0000;

register_bitfields! {
    u32,

    CTRL [
        ENABLE OFFSET(0) NUMBITS(1) [
            True = 1,
            False = 0
        ]
    ],

    INT_MASK [
        INT_OFF OFFSET(0) NUMBITS(1) [
            True = 1,
            False = 0
        ]
    ]
}

#[allow(non_snake_case)]
#[repr(C)]
pub struct Rng {
    CTRL: ReadWrite<u32, CTRL::Register>,         // 0x00
    STATUS: ReadWrite<u32>,                       // 0x04
    DATA: ReadOnly<u32>,                          // 0x08
    __reserved_0: u32,                            // 0x0c
    INT_MASK: ReadWrite<u32, INT_MASK::Register>, // 0x10
}

impl Rng {
    pub fn init(&self) {
        // Turn off interrupts from the RNG
        self.INT_MASK.write(INT_MASK::INT_OFF::True);

        // Set the warmup count in the status, so that it handles it
        // automatically (?)
        self.STATUS.set(WARMUP_COUNT);

        // Enable it
        self.CTRL.modify(CTRL::ENABLE::True);
    }

    pub fn rand(&self) -> u32 {
        // Wait until there's enough entropy
        while self.STATUS.get() & 0xFF00_0000 == 0 {
            spin_loop();
        }

        self.DATA.get()
    }

    /// Generate a random number between 0 (inclusive) and max (exclusive),
    /// uniformly distributed (as far as the rng is uniform).
    pub fn bounded_rand(&self, max: u32) -> u32 {
        // Calculate the smallest value that we should discard
        // If first_unsafe_value = u32::max_value() + 1, that would also be
        // correct here, but that makes the arithmetic much more nasty
        let first_unsafe_value = (u32::max_value() / max) * max;
        loop {
            let n = self.rand();
            if n < first_unsafe_value {
                return n % max;
            }
            // Otherwise it wouldn't be uniform, so try again
        }
    }
}

pub fn get_rng() -> &'static Rng {
    unsafe {
        &*((MMIO_BASE + 0x0010_4000) as *const Rng)
    }
}
