;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Property Accessor Macros for Objects
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
;- 8 Bits
;---------------------------------------------------------
	.macro Property8BitsAccessorsForObjects class, object, property

	.macro object'.Get'property' dest
	GetPropDirect object'.'property, dest
	.endm

	.macro object'.Set'property' value
	.if ''value - 'a
	.if ''value - 'A
	class'.Set'property'HL object, value
	.mexit
	.endif
	.endif
	ld (#object'.'property), A
	.endm

	.endm

;---------------------------------------------------------
;- 16 Bits
;---------------------------------------------------------
	.macro Property16BitsAccessorsForObjects class, object, property

	.macro object'.Get'property'MLSB destMSB, destLSB
	ld HL, (#object'.'property)
	.if ''destMSB - 'h
	.if ''destMSB - 'H
	ld destMSB, H
	.endif
	.endif
	.if ''destLSB - 'l
	.if ''destLSB - 'L
	ld destLSB, L
	.endif
	.endif
	.endm

	.macro object'.Get'property'MSB dest
	GetPropDirect object'.'property + 1, dest
	.endm

	.macro object'.Get'property'LSB dest
	GetPropDirect object'.'property + 1, dest
	.endm

	.macro object'.Set'property' value
	ld HL, value
	ld (#object'.'property), HL
	.endm

	.macro object'.Set'property'MLSB valueMSB, valueMSB
	.if ''valueMSB - 'h
	.if ''valueMSB - 'H
	ld H, valueMSB
	.endif
	.endif
	.if ''valueMSB - 'l
	.if ''valueMSB - 'L
	ld L, valueMSB
	.endif
	.endif
	ld (#object'.'property), HL
	.endm
	
	.macro object'.Set'property'MSB value
	.if ''value - 'a
	.if ''value - 'A
	ld HL, #object'.'property + 1
	ld (HL), value
	.mexit
	.endif
	.endif
	ld (#object'.'property + 1), A
	.endm

	.macro object'.Set'property'LSB value
	.if ''dest - 'a
	.if ''dest - 'A
	class'.Get'property'HL object, dest
	.mexit
	.endif
	.endif
	ld A, (#object'.'property)
	.endm

	.endm

;---------------------------------------------------------
;- Pointers
;---------------------------------------------------------
	.macro PropertyPointersAccessorsForObjects class, property

	.macro class'.Get'property'PtrHL_HL
	GetPropPtrHL_HL class, property
	.endm

	.macro class'.Get'property'PtrHL_IY
	GetPropPtrHL_IY class, property
	.endm

	.macro class'.Get'property'PtrHL object
	GetPropPtrHL object, property
	.endm

	.endm