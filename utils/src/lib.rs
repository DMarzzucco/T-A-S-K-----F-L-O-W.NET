#![feature(renamed_spin_loop)]
#![feature(asm)]

// Don't use the standard library ...
#![no_std]
// ... except for tests
#[cfg(test)]
extern crate std;

pub mod sync;