use crate::peripherals::MMIO_BASE;
use register::{mmio::ReadWrite, register_bitfields, IntLike, RegisterLongName};
use core::ops::Deref;

// Descriptions taken from
// https://github.com/raspberrypi/documentation/files/1888662/BCM2837-ARM-Peripherals.-.Revised.-.V2-1.pdf
register_bitfields! {
    u32,

    //GPFSEL0 [],

    /// GPIO Function Select 1
    GPFSEL1 [
        /// Pin 15
        FSEL15 OFFSET(15) NUMBITS(3) [
            Input = 0b000,
            Output = 0b001,
            RXD0 = 0b100, // UART0     - Alternate function 0
            RXD1 = 0b010  // Mini UART - Alternate function 5

        ],

        /// Pin 14
        FSEL14 OFFSET(12) NUMBITS(3) [
            Input = 0b000,
            Output = 0b001,
            TXD0 = 0b100, // UART0     - Alternate function 0
            TXD1 = 0b010  // Mini UART - Alternate function 5
        ]
    ],

    /*GPFSEL2 [],

    GPFSEL3 [],

    GPFSEL4 [],

    GPFSEL5 [],*/

    GPPUD [
        PUD OFFSET(0) NUMBITS(2) [
            Off = 0,
            PullDown = 0b01,
            PullUp = 0b10
        ]
    ],

    /// GPIO Pull-up/down Clock Register 0
    GPPUDCLK0 [
        /// Pin 15
        PUDCLK15 OFFSET(15) NUMBITS(1) [
            NoEffect = 0,
            AssertClock = 1
        ],

        /// Pin 14
        PUDCLK14 OFFSET(14) NUMBITS(1) [
            NoEffect = 0,
            AssertClock = 1
        ]
    ],
    GPPUDCLK1 [
        /// Pin 15
        PUDCLK32 OFFSET(0) NUMBITS(1) [
            NoEffect = 0,
            AssertClock = 1
        ]
    ]
}

pub struct Reg<T: IntLike, R: RegisterLongName = ()> {
    r: *mut ReadWrite<T, R>
}

impl<T: IntLike, R: RegisterLongName> Reg<T, R> {
    pub const unsafe fn new(r: *mut T) -> Self {
        Reg {
            r: r as *mut ReadWrite<T, R>
        }
    }
}

impl<T: IntLike, R: RegisterLongName> Deref for Reg<T, R> {
    type Target = ReadWrite<T, R>;

    fn deref(&self) -> &ReadWrite<T, R> {
        unsafe {
            &*self.r
        }
    }
}

pub const GPFSEL0: Reg<u32/*, GPFSEL0::Register*/> = unsafe { Reg::new((MMIO_BASE + 0x0020_0000) as *mut u32) };
pub const GPFSEL1: Reg<u32, GPFSEL1::Register> = unsafe { Reg::new((MMIO_BASE + 0x0020_0004) as *mut u32) };
pub const GPFSEL2: Reg<u32/*, GPFSEL2::Register*/> = unsafe { Reg::new((MMIO_BASE + 0x0020_0008) as *mut u32) };
pub const GPFSEL3: Reg<u32/*, GPFSEL3::Register*/> = unsafe { Reg::new((MMIO_BASE + 0x0020_000C) as *mut u32) };
pub const GPFSEL4: Reg<u32/*, GPFSEL4::Register*/> = unsafe { Reg::new((MMIO_BASE + 0x0020_0010) as *mut u32) };
pub const GPFSEL5: Reg<u32/*, GPFSEL5::Register*/> = unsafe { Reg::new((MMIO_BASE + 0x0020_0014) as *mut u32) };

pub const GPPUD: Reg<u32, GPPUD::Register> = unsafe { Reg::new((MMIO_BASE + 0x0020_0094) as *mut u32) };

pub const GPPUDCLK0: Reg<u32, GPPUDCLK0::Register> = unsafe { Reg::new((MMIO_BASE + 0x0020_0098) as *mut u32) };
pub const GPPUDCLK1: Reg<u32, GPPUDCLK1::Register> = unsafe { Reg::new((MMIO_BASE + 0x0020_009C) as *mut u32) };