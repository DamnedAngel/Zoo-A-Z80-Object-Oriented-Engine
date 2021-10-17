;---------------------------------------------------------
;---------------------------------------------------------
;- Reflection Data Operations / Class defined
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
; Get Reflection Property / Class defined
;---------------------------------------------------------
; dest = A, BC, DE, HL, IX, IY, SP
;---------------------------------------------------------
	.macro GetRefClassProp dest, class, prop
	ld dest, (#class'.'prop)
	.endm

;---------------------------------------------------------
; Get Reflection Property Pointer / Class defined
;---------------------------------------------------------
; dest = BC, DE, HL, IX, IY, SP
;---------------------------------------------------------
	.macro GetRefClassPropPtr dest, class, prop
	ld dest, #class'.'prop'
	.endm

;---------------------------------------------------------
; Set Reflection Property / Class defined
;---------------------------------------------------------
; src = A, BC, DE, HL, IX, IY, SP
;---------------------------------------------------------
	.macro SetRefClassProp class, prop, src
	ld (#class'.'prop), src
	.endm
	
;---------------------------------------------------------
;---------------------------------------------------------
;- Reflection Data Operations / Class in IY
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
; Get Reflection Property (8 bits) / Class in IY
;---------------------------------------------------------
; Input:
;	IY			= class
;	dest		= a, b, c, d, e, h, l
; Output:
;	dest		= Property
;---------------------------------------------------------
	.macro GetRefProp8 dest, prop
	ld dest, Class.'prop'.offset(IY)
	.endm

;---------------------------------------------------------
; Get Reflection Property (16 bits) / Class in IY
;---------------------------------------------------------
; Input:
;	IY			= class
;	destXXX		= a, b, c, d, e, h, l
; Output:
;	dest		= Property
;---------------------------------------------------------
	.macro GetRefProp16 destMSB, destLSB, prop
	ld destLSB, Class.'prop'.offset(IY)
	ld destMSB, Class.'prop'.offset+1(IY)
	.endm

;---------------------------------------------------------
; Get Reflection Prop Pointer on HL / Class in IY
;---------------------------------------------------------
; Input:
;	IY			= class
; Output:
;	HL			= Pointer to property
;	BC			= Class.'prop'.offset
;---------------------------------------------------------
	.macro GetRefPropPtr_HL prop
	push iy
	pop hl
    ld bc, #Class.'prop'.offset
    add hl, bc
	.endm

;---------------------------------------------------------
; Set Reflection Property (8 bits) / Class in IY
;---------------------------------------------------------
; Input:
;	IY			= class
;	value		= a, b, c, d, e, h, l, *
;---------------------------------------------------------
	.macro SetRefProp8 prop, value
	ld Class.'prop'.offset(IY), value
	.endm

;---------------------------------------------------------
; Set Reflection Property (16 bits) / Class in IY
;---------------------------------------------------------
; Input:
;	IY			= class
;	valueXXX	= a, b, c, d, e, h, l, *
;---------------------------------------------------------
	.macro SetRefProp16 prop, valueMSB, valueLSB
	ld Class.'prop'.offset(IY), valueLSB
	ld Class.'prop'.offset+1(IY), valueMSB
	.endm