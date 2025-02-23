ENTRY(_boot_cores);

SECTIONS
{
    . = 0x80000;

    .text.boot :
    {
        KEEP(*(.text.boot))
    }

    .text._self_update :
    {
        __self_update_code_start = .;
        KEEP(*(.text.self_update))
        __self_update_code_end = .;
    }

    .text :
    {
        *(.text .text.*)
    }

    .rodata :
    {
        *(.rodata .rodata.*)
    }

    .data :
    {
        __data_start = .;
        *(.data .data.*)
        __data_end = .;
    }

    .bss ALIGN(8):
    {
        __bss_start = .;
        *(.bss .bss.*)
        *(COMMON)
        __bss_end = .;
    }

    __data_static = LOADADDR(.data);

    /DISCARD/ : { *(.comment) *(.gnu*) *(.note*) *(.eh_frame*) }

    __program_end = .;
}