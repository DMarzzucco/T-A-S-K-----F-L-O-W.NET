TARGET = aarch64-unknown-none

# -_- there's no built in way to do recursive wildcards...
# https://stackoverflow.com/a/2483203/2826188
SOURCES = $(shell find -type f -name '*.rs') $(shell find -type f -name '*.S') link.ld

CARGO_OUTPUT = target/$(TARGET)/release/kernel

BUILD_VERSION = --release

.PHONY: all clippy clean objdump nm

all: clean kernel8.img

$(CARGO_OUTPUT): $(SOURCES)
	cargo xrustc $(BUILD_VERSION)

kernel8.img: $(CARGO_OUTPUT)
	cp $< ./kernel8
	cargo objcopy -- --strip-all -O binary $< kernel8.img

clippy:
	cargo xclippy --target=$(TARGET)

clean:
	cargo clean

objdump:
	cargo objdump -- -disassemble -print-imm-hex kernel8

nm:
	cargo nm -- kernel8 | sort