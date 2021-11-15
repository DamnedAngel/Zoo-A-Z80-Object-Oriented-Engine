;	.include "z80unofficial.s"
	.include "zoo.macros.pointers.asm"
	.include "zoo.macros.methods.asm"
	.include "zoo.macros.properties.asm"
	.include "zoo.macros.propertiespointers.asm"

	.globl _zoo_call_hl
	.globl _zoo_error

;---------------------------------------------------------
;---------------------------------------------------------
;- Tools
;---------------------------------------------------------
;---------------------------------------------------------

; -------------------------------
; Calls cross-slot routines via CALSLT.
; -------------------------------
; INPUTS:
; OUTPUTS:
;	AF, BC, DE, HL: Changes
; -------------------------------
	.macro Zoo.CallSlot routine
    push ix
    push iy
    ld iy,(#BIOS_ROMSLT)
    ld ix,#routine
    call #BIOS_CALSLT
    pop iy
    pop ix
	.endm

