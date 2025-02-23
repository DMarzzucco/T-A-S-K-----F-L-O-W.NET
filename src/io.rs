pub mod macros;
pub mod uart;

use crate::peripherals::uart0::Uart;
use self::uart::UartWriter;
use utils::sync::Mutex;
use core::fmt;
use core::mem;

pub use self::macros::*;

pub fn _print(args: fmt::Arguments) {
    match STDOUT.lock().value_mut() {
        None => (),
        Some(uart_writer) => fmt::write(uart_writer, args).unwrap()
    }
}

pub fn set_console(uart: &'static Uart) {
    let mut console = Some(UartWriter::new(uart));
    mem::swap(STDOUT.lock().value_mut(), &mut console);
}

static STDOUT: Mutex<Option<UartWriter>> = Mutex::new(None);
