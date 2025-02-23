use crate::peripherals::MMIO_BASE;
use crate::peripherals::timer;
use crate::peripherals::gpio;
use crate::peripherals::mailbox;
use register::mmio::*;

#[derive(Debug)]
pub enum PowerError {
    MailboxError(mailbox::MailboxError),
}

type Result<T> = ::core::result::Result<T, PowerError>;

const PASSWORD: u32 = 0x5A00_0000;
const PARTITION_HALT: u32 = 0x0000_0555;
const REBOOT_WATCHDOG_TIMEOUT: u32 = 10;

#[allow(non_snake_case)]
#[repr(C)]
pub struct PowerManager {
    RSTC: ReadWrite<u32>, // 0x1C
    RSTS: ReadWrite<u32>, // 0x20
    WDOG: ReadWrite<u32>  // 0x24
}

impl PowerManager {
    pub fn shutdown(&self) -> Result<!> {
        // Shut down all of the devices
        for device in mailbox::Device::values() {
            if let Err(e) = mailbox::set_power_state(device, false, false) {
                return Err(PowerError::MailboxError(e));
            }
        }

        // Reset all the GPIO pins back to their default function
        gpio::GPFSEL0.set(0);
        gpio::GPFSEL1.set(0);
        gpio::GPFSEL2.set(0);
        gpio::GPFSEL3.set(0);
        gpio::GPFSEL4.set(0);
        gpio::GPFSEL5.set(0);
        // Set all the pins to be input (unpowered) - see doc page 101
        // First write the desired control signal, then wait 150 cycles
        gpio::GPPUD.write(gpio::GPPUD::PUD::Off);
        timer::sleep_cycles(150);
        // Then mask the GPPUDCLK register to set the pins that should be
        // updated. Here, that's all the pins
        gpio::GPPUDCLK0.set(0xFFFF_FFFF);
        gpio::GPPUDCLK1.set(0x03FF_FFFF);
        // Now wait another 150 cycles for it to take effect
        timer::sleep_cycles(150);
        // Then reset the mask to stop sending the signal
        gpio::GPPUDCLK0.set(0);
        gpio::GPPUDCLK1.set(0);
        // Now the GPIO pins are shut down

        // Tell the watchdog to halt when it reboots
        self.RSTS.set(self.RSTS.get() | PASSWORD | PARTITION_HALT);

        // Then finally reboot it
        self.reboot();
    }

    #[allow(clippy::empty_loop)]
    pub fn reboot(&self) -> ! {
        self.WDOG.set(REBOOT_WATCHDOG_TIMEOUT | PASSWORD);
        self.RSTC.set(self.RSTC.get() & 0xFFFF_FFCF | PASSWORD | 0x0000_0020);

        loop {}
    }
}

pub fn get_power_manager() -> &'static PowerManager {
    unsafe{ &*((MMIO_BASE + 0x0010_001C) as *const PowerManager) }
}
