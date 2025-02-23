use crate::peripherals::mailbox;
use core::sync::atomic::{fence, Ordering};
use core::convert::TryInto;

const MAILBOX_BUFFER_SIZE: usize = 42;

#[derive(Debug)]
#[allow(dead_code)]
pub enum PropertyTag {
    AllocateFrameBuffer = 0x0004_0001,
    ReleaseFrameBuffer = 0x0004_8001,
    BlankScreen = 0x0004_0002,

    /// Get the resolution of the allocated frame buffer
    GetPhysicalSize = 0x0004_0003,
    TestPhysicalSize = 0x0004_4003,
    SetPhysicalSize = 0x0004_8003,

    /// Get the portion of the frame buffer that is sent to the display
    GetVirtualSize = 0x0004_0004,
    TestVirtualSize = 0x0004_4004,
    SetVirtualSize = 0x0004_8004,

    /// Get the number of bits per pixel
    GetDepth = 0x0004_0005,
    TestDepth = 0x0004_4005,
    SetDepth = 0x0004_8005,

    GetPixelOrder = 0x0004_0006,
    TestPixelOrder = 0x0004_4006,
    SetPixelOrder = 0x0004_8006,
    
    GetAlphaMode = 0x0004_0007,
    TestAlphaMode = 0x0004_4007,
    SetAlphaMode = 0x0004_8007,

    /// Get the number of bytes per line
    GetPitch = 0x0004_0008,

    /// Get the offset of the virtual frame buffer within the physical one
    GetVirtualOffset = 0x0004_0009,
    TestVirtualOffset = 0x0004_4009,
    SetVirtualOffset = 0x0004_8009,

    GetOverscan = 0x0004_000A,
    TestOverscan = 0x0004_400A,
    SetOverscan = 0x0004_800A,

    GetPalette = 0x0004_000B,
    TestPalette = 0x0004_400B,
    SetPalette = 0x0004_800B,

    SetCursorInfo = 0x0000_8010,
    SetCursorState = 0x0000_8011,
}

#[repr(u32)]
#[allow(dead_code)]
pub enum PixelOrder {
    BGR = 0x0,
    RGB = 0x1,
}

#[repr(u32)]
#[allow(dead_code)]
pub enum AlphaMode {
    /// When enabled, an alpha value of 0 = fully opaque, 255 = fully transparent
    Enabled = 0x0,
    /// When reversed, alpha value of 0 = fully transparent, 255 = fully opaque
    Reversed = 0x1,
    /// When disabled, all alpha values are fully opaque
    Disabled = 0x2,
}

#[derive(Debug)]
pub enum FrameBufferCreationError {
    BadRequest,
    BadResponse(u32),
    RequestRejected(PropertyTag),
}

#[repr(C)]
#[repr(align(16))]
struct MailboxBuffer {
    buffer: [u32; MAILBOX_BUFFER_SIZE]
}

#[repr(C)]
pub struct FrameBuffer {
    /// Pointer to the start of the buffer
    buffer: *mut u32,
    /// The allocated length of the buffer
    buffer_size: usize,
    /// The length of each row as allocated in memory, in bytes
    pitch: usize,
    /// The width of the frame buffer
    width: usize,
    /// The height of the frame buffer
    height: usize,
}

impl FrameBuffer {
    #[allow(clippy::identity_op)]
    pub fn new(width: u32, height: u32) -> Result<FrameBuffer, FrameBufferCreationError> {
        let buffer = MailboxBuffer {
            buffer: [
                // Header information
                MAILBOX_BUFFER_SIZE as u32 * 4,         // 0
                mailbox::RequestCode::Request as u32,   // 1

                PropertyTag::SetPhysicalSize as u32,    // 2
                2 * 4, // Value buffer size             // 3
                2 * 4, // This is a request             // 4
                width, // Desired buffer dimensions     // 5
                height,                                 // 6

                PropertyTag::SetVirtualSize as u32,     // 7
                2 * 4,                                  // 8
                2 * 4,                                  // 9
                width, // Screen dimensions             // 10
                height,                                 // 11

                PropertyTag::SetVirtualOffset as u32,   // 12
                2 * 4,                                  // 13
                2 * 4,                                  // 14
                0, // Frame buffer in top left corner   // 15
                0,                                      // 16

                PropertyTag::SetDepth as u32,           // 17
                1 * 4,                                  // 18
                1 * 4,                                  // 19
                32, // 32 bits per pixel                // 20

                // RGB doesn't seem to work, so make sure it uses BGR
                PropertyTag::SetPixelOrder as u32,      // 21
                1 * 4,                                  // 22
                1 * 4,                                  // 23
                PixelOrder::BGR as u32,                 // 24

                PropertyTag::AllocateFrameBuffer as u32,// 25
                2 * 4,                                  // 26
                1 * 4,                                  // 27
                4096, // Alignment in bytes             // 28
                0, // Padding to allow for response     // 29

                PropertyTag::GetPitch as u32,           // 30
                1 * 4,                                  // 31
                0,                                      // 32
                0, // Padding for response              // 33

                PropertyTag::SetOverscan as u32,        // 34
                4 * 4,                                  // 35
                4 * 4,                                  // 36
                0, 0, 0, 0,                             // 37, 38, 39, 40

                mailbox::Tag::Last as u32,              // 41
            ]
        };

        let ptr = &buffer.buffer as *const u32 as usize as u32;
        mailbox::mailbox_call(mailbox::Channel::PropertyTagsVC, ptr & !0x0F);

        match buffer.buffer[1].try_into() {
            Ok(mailbox::ResponseCode::Success) => (),
            Ok(mailbox::ResponseCode::Error) => {
                return Err(FrameBufferCreationError::BadRequest);
            }
            Err(n) => {
                return Err(FrameBufferCreationError::BadResponse(n));
            }
        };

        if (buffer.buffer[5], buffer.buffer[6]) != (width, height) {
            return Err(FrameBufferCreationError::RequestRejected(PropertyTag::SetPhysicalSize));
        }
        if (buffer.buffer[10], buffer.buffer[11]) != (width, height) {
            return Err(FrameBufferCreationError::RequestRejected(PropertyTag::SetVirtualSize));
        }
        if buffer.buffer[20] != 32 {
            return Err(FrameBufferCreationError::RequestRejected(PropertyTag::SetDepth));
        }
        if buffer.buffer[24] != (PixelOrder::BGR as u32) {
            return Err(FrameBufferCreationError::RequestRejected(PropertyTag::SetPixelOrder));
        }
        if buffer.buffer[28] == 0 || buffer.buffer[29] == 0 {
            return Err(FrameBufferCreationError::RequestRejected(PropertyTag::AllocateFrameBuffer));
        }

        // All good, save and return
        Ok(FrameBuffer {
            buffer: (buffer.buffer[28] & 0x3FFF_FFFF) as usize as *mut u32,
            buffer_size: buffer.buffer[29] as usize,
            pitch: buffer.buffer[33] as usize,
            width: width as usize,
            height: height as usize,
        })
    }

    pub fn draw(&mut self) {
        for y in 0..self.height {
            for x in 0..self.width {
                self.set_pixel(x, y, (x / 8) as u8, (y / 4) as u8, 0, 255);
            }
        }

        fence(Ordering::Release);
    }

    #[inline]
    pub fn set_pixel(&mut self, x: usize, y: usize, r: u8, g: u8, b: u8, a: u8) {
        assert!(x < self.width && y < self.height);
        let val = u32::from(r) | (u32::from(g) << 8) | (u32::from(b) << 16) | (u32::from(a) << 24);
        unsafe {
            self.buffer.add(y * self.pitch / 4 + x).write_volatile(val);
        }
    }
}
