# This file was generated automatically on 29-10-18 12:55
# Builder: Builder 1.0.0.0
# Project: Project1
# Target board: Olimex-T795
# Processor: PIC32MX795F512H
# Framework: Core32

# Variables
OUTDIR=Output
OBJDIR=build/obj
NAME=hello

# Source files for Project1
SRC += src/test.c

# Toolset (xc32)
TOOLSETPATH=C:\Program Files (x86)\Microchip\xc32\v1.44\bin
CC=$(TOOLSETPATH)/xc32-gcc
LD=$(TOOLSETPATH)/xc32-ld
BIN2HEX=$(TOOLSETPATH)/xc32-bin2hex

# Source files for Core32
SRC += build/ext/core32/src/adc.c
SRC += build/ext/core32/src/cn.c
SRC += build/ext/core32/src/dma.c
SRC += build/ext/core32/src/extint.c
SRC += build/ext/core32/src/i2c.c
SRC += build/ext/core32/src/nvm.c
SRC += build/ext/core32/src/oc.c
SRC += build/ext/core32/src/spi.c
SRC += build/ext/core32/src/sysmain.c
SRC += build/ext/core32/src/system.c
SRC += build/ext/core32/src/tasks.c
SRC += build/ext/core32/src/timers.c
SRC += build/ext/core32/src/uart.c

# Include directories for Core32
CFLAGS += -Ibuild/ext/core32/inc

# Include directories for Project1
CFLAGS += -I../hello

# Definitions for Core32
CFLAGS += -DFRAMEWORK_CORE32

# Definitions for Olimex-T795
CFLAGS += -DBOARD_OLIMEX_T795

# Definitions for xc32
CFLAGS += -DTOOLSET_XC32
CFLAGS += -DCORE32_HOST_WINDOWS

# Definitions for PIC32MX795F512H
CFLAGS += -DCPU_PIC32MX795F512H

# Automatically generated files
CFLAGS += -Ibuild\gen\Core32

# Library flags
LDFLAGS=

OBJ=$(addprefix $(OBJDIR)/,$(patsubst %.c,%.o,$(SRC)))

all: $(OBJ)
	@if not exist "$(OUTDIR)" md "$(OUTDIR)"
	$(CC) -o $(OUTDIR)/$(NAME).bin $(OBJ) $(LDFLAGS)
	$(BIN2HEX) $(OUTDIR)/$(NAME).bin

clean: 
	rmdir /s /q $(OBJDIR)

fclean: clean
	del $(OUTDIR)/$(NAME).bin
	del $(OUTDIR)/$(NAME).hex

$(OBJDIR)/%.o: %.c
	@if not exist "$(dir $@)" md "$(dir $@)"
	$(CC) $(CFLAGS) -o $@ -c $<

re: fclean all

