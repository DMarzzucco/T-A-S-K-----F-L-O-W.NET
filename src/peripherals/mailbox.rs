use crate::peripherals::MMIO_BASE;
use register::{mmio::{ReadOnly, WriteOnly}, register_bitfields};
use core::hint::spin_loop;
use core::sync::atomic::{fence, Ordering};
use core::slice;
use core::convert::{TryFrom, TryInto};
use macros::*;

register_bitfields!{
    u32,
    STATUS [
        FULL    OFFSET(31) NUMBITS(1) [],
        EMPTY   OFFSET(30) NUMBITS(1) []
    ]
}

#[allow(non_snake_case)]
#[repr(C)]
pub struct Mailbox0Registers {
    DATA: ReadOnly<u32>,                        // 0x00
    __reserved_0: [u32; 3],                     // 0x04
    PEEK: ReadOnly<u32>,                        // 0x10
    SENDER: ReadOnly<u32>,                      // 0x14
    STATUS: ReadOnly<u32, STATUS::Register>,    // 0x18
    CONFIG: ReadOnly<u32>,                      // 0x1C
}


#[allow(non_snake_case)]
#[repr(C)]
pub struct Mailbox1Registers {
    DATA: WriteOnly<u32>,                       // 0x20
    __reserved_0: [u32; 3],                     // 0x24
    PEEK: ReadOnly<u32>,                        // 0x30
    SENDER: ReadOnly<u32>,                      // 0x34
    STATUS: ReadOnly<u32, STATUS::Register>,    // 0x38
    CONFIG: ReadOnly<u32>,                      // 0x3C
}

fn get_mailbox_0() -> &'static Mailbox0Registers {
    unsafe {
        &*((MMIO_BASE + 0xB880) as *const Mailbox0Registers)
    }
}
fn get_mailbox_1() -> &'static Mailbox1Registers {
    unsafe {
        &*((MMIO_BASE + 0xB880 + 0x20) as *const Mailbox1Registers)
    }
}

#[derive(Debug)]
pub enum MailboxError {
    ResponseError,
    SizeError(u32),
    OverflowError,
    UnknownError,
}
pub type Result<T> = ::core::result::Result<T, MailboxError>;

#[derive(Copy, Clone)]
#[allow(dead_code)]
#[repr(u32)]
pub enum Channel {
    PowerManagement = 0,
    FrameBuffer = 1,
    VirtualUART = 2,
    VCHIQ = 3,
    LEDs = 4,
    Buttons = 5,
    TouchScreen = 6,
    PropertyTagsVC = 8,
    PropertyTagsCPU = 9
}

#[allow(dead_code)]
#[repr(u32)]
pub enum Tag {
    Last = 0,
    GetFirmware = 0x0000_0001,
    GetModel = 0x0001_0001,
    GetRevision = 0x0001_0002,
    GetMac = 0x0001_0003,
    GetSerial = 0x0001_0004,
    GetArmMemory = 0x0001_0005,
    GetVCMemory = 0x0001_0006,
    GetClocks = 0x0001_0007,

    SetPowerState = 0x0002_8001,

    SetClockRate = 0x0003_8002,
}

#[derive(PartialEq, Eq, Debug)]
#[repr(u32)]
pub enum Clock {
    UART = 0x0000_0002,
}

#[derive(IterableEnum)]
#[repr(u32)]
pub enum Device {
    SDCard = 0x0,
    UART0 = 0x1,
    UART1 = 0x2,
    UsbHcd = 0x3,
    I2C0 = 0x4,
    I2C1 = 0x5,
    I2C2 = 0x6,
    SPI = 0x7,
    CCP2TX = 0x8,
}

#[derive(TryFrom)]
#[repr(u32)]
pub enum ResponseCode {
    Success = 0x8000_0000,
    Error = 0x8000_0001,
}

#[repr(u32)]
pub enum RequestCode {
    Request = 0x0,
}

const MESSAGE_SIZE: usize = 12;

/// Buffer for the messages to exchange with the GPU
#[repr(C)]
#[repr(align(16))]
pub struct Message {
    buffer: [u32; MESSAGE_SIZE],
}

impl Message {
    /// Construct a new message to send
    pub fn new() -> Message {
        let mut msg = Message {
            buffer: [0; MESSAGE_SIZE]
        };

        msg.set_message_size(MESSAGE_SIZE * 4);
        msg.set_request_code(RequestCode::Request);
        msg.set_value_buffer_size((MESSAGE_SIZE - 6) * 4);

        msg
    }

    #[inline]
    pub fn set_tag(&mut self, val: Tag) {
        self.buffer[2] = val as u32;
    }

    #[inline]
    pub fn set_query(&mut self, query: &[u32]) {
        self.buffer[4] = (query.len() * 4) as u32;
        let mut index = 5;
        for value in query {
            self.buffer[index] = *value;
            index += 1;
        }
    }

    #[inline]
    fn set_message_size(&mut self, val: usize) {
        self.buffer[0] = val as u32;
    }

    #[inline]
    fn set_request_code(&mut self, code: RequestCode) {
        self.buffer[1] = code as u32;
    }

    #[inline]
    fn set_value_buffer_size(&mut self, val: usize) {
        self.buffer[3] = val as u32;
    }

    #[inline]
    pub fn get_response_code(&self) -> Result<ResponseCode> {
        match self.buffer[1].try_into() {
            Err(_) => Err(MailboxError::UnknownError),
            Ok(v) => Ok(v)
        }
    }

    #[inline]
    pub fn is_response(&self) -> bool {
        self.buffer[4] & 0x8000_0000 != 0
    }

    #[inline]
    pub fn get_response_length(&self) -> u32 {
        self.buffer[4] & 0x7FFF_FFFF
    }

    #[inline]
    pub fn get_response(&self) -> &[u32] {
        self.buffer.split_at(5).1.split_at(5 + (self.get_response_length() as usize / 4)).0
    }

    pub fn send(&mut self, tag: Tag, query: &[u32], expected_len: u32) -> Result<()> {
        if expected_len > ((MESSAGE_SIZE - 6) * 4) as u32 {
            return Err(MailboxError::OverflowError);
        }

        self.set_tag(tag);
        self.set_query(query);

        // The address of the buffer has to be coerced into a 32 bit pointer
        let buf_ptr = self as *mut Message as u32;

        // The message is the upper 28 bits of the pointer in the upper 28 bits
        // of the message, and the channel in the lower 4 bits
        let msg: u32 = buf_ptr & !0x0F;

        // send it
        mailbox_call(Channel::PropertyTagsVC, msg);

        match self.get_response_code()? {
            ResponseCode::Success => {
                if !self.is_response() {
                    Err(MailboxError::UnknownError)
                } else {
                    let size = self.get_response_length();
                    if size != expected_len {
                        Err(MailboxError::SizeError(size))
                    } else {
                        Ok(())
                    }
                }
            }
            ResponseCode::Error => Err(MailboxError::ResponseError),
        }
    }
}

/// Get the firmware revision of this board
pub fn get_firmware_revision() -> Result<u32> {
    let mut message = Message::new();
    message.send(Tag::GetFirmware, &[], 4)?;
    Ok(message.get_response()[0])
}

/// Get the mac address of this board
pub fn get_mac() -> Result<[u8; 6]> {
    let mut message = Message::new();
    message.send(Tag::GetMac, &[], 6)?;
    let mac: &[u8] = unsafe {
        slice::from_raw_parts(message.get_response().as_ptr() as *const u8, 6)
    };
    Ok([mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]])
}

/// Get the serial number of this board
pub fn get_serial() -> Result<u64> {
    let mut message = Message::new();
    message.send(Tag::GetSerial, &[], 8)?;
    Ok(u64::from(message.get_response()[0]) + (u64::from(message.get_response()[1]) << 32))
}

/// Get the base address and size of the ram allocated to the ARM core
pub fn get_memory_range() -> Result<(u32, u32)> {
    let mut message = Message::new();
    message.send(Tag::GetArmMemory, &[], 8)?;
    Ok((message.get_response()[0], message.get_response()[1]))
}

pub fn set_power_state(device: Device, state: bool, wait_for_completion: bool) -> Result<bool> {
    let mut message = Message::new();
    let flags = u32::from(state) + (u32::from(wait_for_completion) << 1);
    message.send(Tag::SetPowerState, &[device as u32, flags], 8)?;
    Ok(message.get_response()[1] & 0x1 == 1)
}

pub fn set_clock_rate(clock: Clock, rate: u32, skip_setting_turbo: u32) -> Result<u32> {
    let mut message = Message::new();
    message.send(Tag::SetClockRate, &[clock as u32, rate, skip_setting_turbo], 8)?;
    Ok(message.get_response()[1])
}


pub fn mailbox_call(channel: Channel, msg: u32) {
    let msg = msg | (channel as u32);

    // Wait for there to be space in the mailbox (I think that should always
    // be the case anyway)
    while get_mailbox_1().STATUS.is_set(STATUS::FULL) {
        spin_loop();
    }

    // Make sure no memory operations cross this point.
    // The processor on the pi executes in-order, so it doesn't need to be
    // exposed as a fence in the actual instructions
    fence(Ordering::Release);

    // Send it!
    get_mailbox_1().DATA.set(msg);

    // Wait for the response
    let mailbox0 = get_mailbox_0();
    loop {
        if !mailbox0.STATUS.is_set(STATUS::EMPTY) {
            // Peek at the message and see if it's for us
            let resp: u32 = mailbox0.DATA.get();
            if (resp & !0x0F) == (msg & !0x0F) {
                // It is for us, so pop it off then check whether it's the
                // response that we care about
                if (resp & 0x0F) == (channel as u32) {
                    // This is the one we're interested in
                    // First we need to insert a barrier so we can't speculate this
                    fence(Ordering::Acquire);

                    return;
                }
            }
        }

        spin_loop();
    }
}