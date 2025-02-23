use crate::peripherals::uart0::get_uart;
use crate::peripherals::power;

use core::fmt;
use core::panic::PanicInfo;

#[panic_handler]
#[allow(unused_must_use)]
#[cfg(not(test))]
fn panic(info: &PanicInfo) -> ! {
    let mut uart = unsafe {&mut*(get_uart() as *const crate::peripherals::uart0::Uart as *mut crate::peripherals::uart0::Uart)};
    uart.init();
    
    fmt::write(&mut uart, format_args!("{:?}", info));

    power::get_power_manager().reboot()
}
