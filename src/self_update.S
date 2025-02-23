.section ".text.self_update"

.global _self_update
.global _test_add

_self_update:
    // The start address should be in r0, the length should be in r1, and the
    // base UART address should be in r2
    // Assumes that UART0 has been initialised, so we can just read that length
    // from the register
    
    // Make sure we still have data to copy
    CBZ x1, done

wait:
    // Check whether there's anything to read (32 bit load)
    LDUR w3, [x2, #0x18]
    TBNZ w3, #4, wait // Loop if the RXFE (read buffer empty) flag is set

    // Otherwise, read the character from the buffer, and write it to the
    // appropriate memory location
    LDR w3, [x2]
    STRB w3, [x0]

    // Update the start address and length
    SUB x1, x1, #1
    ADD x0, x0, #1

    // Loop
    b _self_update

done:
    // Nothing else needs zeroing because the bootstrap script can't rely on
    // anything (the stack start uninitialised etc).
    // Therefore, we just need to flush the instruction cache and jump to the
    // entrypoint. We also need to wake up all the others cores, and in future
    // might need to change their instruction pointers too.

    IC IALLU // Clear instruction cache
    ISB // Force pipeline flush
    SEV // Signal event to wake up other cores
    MOV w0, #0x80000
    BR x0 // Jump to the entry point