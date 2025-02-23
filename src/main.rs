#![no_std]
#![no_main]
#![feature(asm)]
#![feature(global_asm)]
#![feature(renamed_spin_loop)]
#![feature(const_fn)]
#![feature(const_raw_ptr_deref)]
#![feature(never_type)]
#![feature(format_args_nl)]

mod display;
mod peripherals;
mod io;
mod panic_handler;
mod self_update;

fn entry() -> ! {
    let uart = peripherals::uart0::get_uart();

    uart.init().unwrap();
    
    io::set_console(uart);

    println!("Hello!");

    match peripherals::mailbox::get_mac() {
        Ok(mac) => {
            println!("Mac address: {:02X}:{:02X}:{:02X}:{:02X}:{:02X}:{:02X}",
                mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]
            );
        },
        Err(e) => println!("{:?}", e),
    };

    match peripherals::mailbox::get_serial() {
        Ok(serial) => {
            println!("Serial number: {:X}", serial);
        },
        Err(e) => println!("{:?}", e),
    }

    match peripherals::mailbox::get_memory_range() {
        Ok((base, length)) => {
            println!("Memory size: {:#X}B. Base: {:#X}", length, base);
        }
        Err(e) => println!("{:?}", e),
    }

    let rand = peripherals::random::get_rng();
    rand.init();

    let mut frame_buffer = display::frame_buffer::FrameBuffer::new(1920, 1080).unwrap();
    frame_buffer.draw();

    loop {
        let c = uart.getc();
        if c == '^' {
            if let Err(e) = self_update::self_update(uart) {
                println!("{:?}", e);
            }
        } else if c == '\n' {
            println!();
        } else if c == 'r' {
            println!("{:#x}", rand.rand());
        } else if c == 'R' {
            peripherals::power::get_power_manager().reboot();
        } else if c == 's' {
            peripherals::power::get_power_manager().shutdown().unwrap();
        } else {
            print!("{}", c);
        }
    }
}

init::entry!(entry);
