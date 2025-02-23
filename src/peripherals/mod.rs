pub mod gpio;
pub mod mailbox;
pub mod power;
pub mod random;
pub mod timer;
pub mod uart0;

const MMIO_BASE: usize = 0x3F00_0000;