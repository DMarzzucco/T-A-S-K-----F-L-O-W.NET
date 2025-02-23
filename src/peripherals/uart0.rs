use crate::peripherals::MMIO_BASE;
use crate::peripherals::timer::sleep_cycles;
use crate::peripherals::gpio;
use crate::peripherals::mailbox;
use core::hint::spin_loop;
use core::fmt::Write;
use register::{mmio::*, register_bitfields};

// PL011 UART registers.
//
// Descriptions taken from
// https://github.com/raspberrypi/documentation/files/1888662/BCM2837-ARM-Peripherals.-.Revised.-.V2-1.pdf
register_bitfields! {
    u32,

    /// Flag Register
    FR [
        /// Transmit FIFO full. The meaning of this bit depends on the
        /// state of the FEN bit in the UARTLCR_ LCRH Register. If the
        /// FIFO is disabled, this bit is set when the transmit
        /// holding register is full. If the FIFO is enabled, the TXFF
        /// bit is set when the transmit FIFO is full.
        TXFF OFFSET(5) NUMBITS(1) [],

        /// Receive FIFO empty. The meaning of this bit depends on the
        /// state of the FEN bit in the UARTLCR_H Register. If the
        /// FIFO is disabled, this bit is set when the receive holding
        /// register is empty. If the FIFO is enabled, the RXFE bit is
        /// set when the receive FIFO is empty.
        RXFE OFFSET(4) NUMBITS(1) []
    ],

    /// Integer Baud rate divisor
    IBRD [
        /// Integer Baud rate divisor
        IBRD OFFSET(0) NUMBITS(16) []
    ],

    /// Fractional Baud rate divisor
    FBRD [
        /// Fractional Baud rate divisor
        FBRD OFFSET(0) NUMBITS(6) []
    ],

    /// Line Control register
    LCRH [
        /// Word length. These bits indicate the number of data bits
        /// transmitted or received in a frame.
        WLEN OFFSET(5) NUMBITS(2) [
            FiveBit = 0b00,
            SixBit = 0b01,
            SevenBit = 0b10,
            EightBit = 0b11
        ]
    ],

    /// Control Register
    CR [
        /// Receive enable. If this bit is set to 1, the receive
        /// section of the UART is enabled. Data reception occurs for
        /// UART signals. When the UART is disabled in the middle of
        /// reception, it completes the current character before
        /// stopping.
        RXE    OFFSET(9) NUMBITS(1) [
            Disabled = 0,
            Enabled = 1
        ],

        /// Transmit enable. If this bit is set to 1, the transmit
        /// section of the UART is enabled. Data transmission occurs
        /// for UART signals. When the UART is disabled in the middle
        /// of transmission, it completes the current character before
        /// stopping.
        TXE    OFFSET(8) NUMBITS(1) [
            Disabled = 0,
            Enabled = 1
        ],

        /// UART enable
        UARTEN OFFSET(0) NUMBITS(1) [
            /// If the UART is disabled in the middle of transmission
            /// or reception, it completes the current character
            /// before stopping.
            Disabled = 0,
            Enabled = 1
        ]
    ],

    /// Interupt Clear Register
    ICR [
        /// Meta field for all pending interrupts
        ALL OFFSET(0) NUMBITS(11) []
    ]
}

pub fn get_uart() -> &'static Uart {
    unsafe {
        &*((MMIO_BASE + 0x0020_1000) as *const Uart)
    }
}

#[derive(Debug)]
pub enum UartError {
    MailboxError(mailbox::MailboxError),
}
pub type Result<T> = ::core::result::Result<T, UartError>;

#[allow(non_snake_case)]
#[repr(C)]
pub struct Uart {
    DR: ReadWrite<u32>,                   // 0x00
    __reserved_0: [u32; 5],               // 0x04
    FR: ReadOnly<u32, FR::Register>,      // 0x18
    __reserved_1: [u32; 2],               // 0x1c
    IBRD: WriteOnly<u32, IBRD::Register>, // 0x24
    FBRD: WriteOnly<u32, FBRD::Register>, // 0x28
    LCRH: WriteOnly<u32, LCRH::Register>, // 0x2C
    CR: WriteOnly<u32, CR::Register>,     // 0x30
    __reserved_2: [u32; 4],               // 0x34
    ICR: WriteOnly<u32, ICR::Register>,   // 0x44
}

impl Uart {
    pub fn init(&self) -> Result<()> {
        // Turn off the UART so we can configure it
        self.CR.set(0);

        // Set the UART clock speed
        if let Err(e) = mailbox::set_clock_rate(mailbox::Clock::UART, 4_000_000, 0) {
            return Err(UartError::MailboxError(e));
        }

        gpio::GPFSEL1.modify(gpio::GPFSEL1::FSEL14::TXD0 + gpio::GPFSEL1::FSEL15::RXD0);
        gpio::GPPUD.set(0);

        sleep_cycles(150);

        gpio::GPPUDCLK0.write(
            gpio::GPPUDCLK0::PUDCLK14::AssertClock + gpio::GPPUDCLK0::PUDCLK15::AssertClock
        );

        sleep_cycles(150);

        gpio::GPPUDCLK0.set(0);

        // Set the baud rate to 11520 baud
        self.ICR.write(ICR::ALL::CLEAR);
        self.IBRD.set(2);
        self.FBRD.set(11);
        self.LCRH.write(LCRH::WLEN::EightBit);
        self.CR.write(CR::UARTEN::Enabled + CR::TXE::Enabled + CR::RXE::Enabled);

        Ok(())
    }

    pub fn send(&self, c: char) {
        // Wait for the buffer to have enough space
        while self.FR.is_set(FR::TXFF) {
            spin_loop();
        }

        self.DR.set(c as u32);
    }

    pub fn puts(&self, string: &str) {
        for c in string.chars() {
            if c == '\n' {
                self.send('\r');
            }
            self.send(c);
        }
    }

    pub fn getc(&self) -> char {
        // Wait for there to be a character available
        while self.FR.is_set(FR::RXFE) {
            spin_loop();
        }

        let ret = self.DR.get() as u8 as char;

        if ret == '\r' {
            '\n'
        } else {
            ret
        }
    }

    pub fn send_hex_u32(&self, n: u32) {
        let mut chars: [u8; 8] = [0; 8];
        for i in 0..8 {
            let mut v: u8 = ((n >> (i * 4)) & 0xF) as u8;
            if v >= 10 {
                v += 55;
            } else {
                v += 48;
            }
            chars[7 - i] = v;
        }

        for i in 0..8 {
            self.send(chars[i] as char);
        }
    }

    pub fn send_hex_u8(&self, n: u8) {
        let mut chars: [u8; 2] = [0; 2];
        for i in 0..2 {
            let mut v: u8 = ((n >> (i * 4)) & 0xF) as u8;
            if v >= 10 {
                v += 55;
            } else {
                v += 48;
            }
            chars[1 - i] = v;
        }

        for i in 0..2 {
            self.send(chars[i] as char);
        }
    }

    pub fn send_hex_u64(&self, n: u64) {
        self.send_hex_u32((n >> 32) as u32);
        self.send_hex_u32(n as u32);
    }

    pub fn newline(&self) {
        self.send('\r');
        self.send('\n');
    }
}

impl Write for Uart {
    fn write_str(&mut self, s: &str) -> ::core::result::Result<(), ::core::fmt::Error> {
        self.puts(s);
        Ok(())
    }
}
