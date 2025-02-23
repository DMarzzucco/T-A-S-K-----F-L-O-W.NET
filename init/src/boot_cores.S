.section ".text.boot"

.global _boot_cores

_boot_cores:
    // Read the CPU id from the processor special register into x1
    mrs     x1, mpidr_el1
    and     x1, x1, #0xFF // Just interested in the lower 8 bits (the rest have different information)
    cbz     x1, core0
halt: // Non-zero cores fall through to here and are stopped
    wfe
    b       halt

core0: // Core zero branches to here
    // Check the current execution level
    mrs     x1, CurrentEL
    cmp     x1, #0b0100 // EL1
    b.eq    el1 // If we're in EL1, we can skip the EL2 initialisation
    cmp     x1, #0b1000 // EL2
    b.ne    halt // If we're not in EL1 or EL2, we have to give up

el2:
    // We are in EL2, so we need to tell the processor what to allow for EL1
    // Allow access to all the timer and counter registers
    // https://developer.arm.com/docs/ddi0595/latest/aarch64-system-registers/cnthctl_el2
    mov     x1, #0b11
    msr     cnthctl_el2, x1
    // Also don't have an offset from the actual value
    msr     cntvoff_el2, xzr

    // Set EL1 to use AArch64 mode
    // https://developer.arm.com/docs/ddi0595/latest/aarch64-system-registers/hcr_el2
    mov     x1, #0x80000000
    msr     hcr_el2, x1

    // Now set up the saved program status register with an appropriate
    // interrupt mask so that we can drop to EL1
    // https://developer.arm.com/docs/ddi0595/latest/aarch64-system-registers/spsr_el2
    mov     x1, #0b1111000101
    msr     spsr_el2, x1

    // Set the stack pointer
    bl      get_stack_ptr
    msr     sp_el1, x0

    // Set the exception return pointer to our EL1 entry point
    ldr     x1, =el1_main
    msr     elr_el2, x1

    // Then say goodbye to hypervisor privileges
    eret

el1:
    // If we were already in el1, we need to set the stack up ourself
    bl      get_stack_ptr
    mov     sp, x0 // Let the stack build up from the location of _boot_cores

el1_main:
    // Jump to rust, and include a return pointer (though it should never return)
    bl      reset

    // If we return somehow, halt this core too
    b       halt

get_stack_ptr:
    ldr     x0, =__program_end
    add     x0, x0, #0x1000 // Allocate 4 KiB for the stack
    mov     x3, #0xFFFFFFFFFFFFFF00 //Align the stack pointer to 64 bits
    and     x0, x0, x3
    ret