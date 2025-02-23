use crate::peripherals::uart0::Uart;
use core::fmt;

pub struct UartWriter {
    uart: &'static Uart,
}

impl UartWriter {
    pub fn new(uart: &'static Uart) -> UartWriter {
        UartWriter {
            uart: uart,
        }
    }
}

impl fmt::Write for UartWriter {
    fn write_str(&mut self, s: &str) -> Result<(), fmt::Error> {
        self.uart.puts(s);
        Ok(())
    }
}
