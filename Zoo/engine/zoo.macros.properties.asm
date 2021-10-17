;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Properties Macros
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
; Get Property (8 bits) / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	dest		= a, b, c, d, e, h, l
; Output:
;	dest		= Property value
;---------------------------------------------------------
	.macro GetProp8_IY class, prop, dest
	ld dest, class'.'prop'.offset(IY)
	.endm

;---------------------------------------------------------
; Get Property (8 bits) / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	class		= Class name
;	prop		= Property name
;	dest		= a, b, c, d, e, h, l
; Output:
;	dest		= Property value
;---------------------------------------------------------
	.macro GetProp8 object, class, prop, dest
	Zoo.SetObj ^/object/
	GetProp8_IY dest, class, prop
	.endm

;---------------------------------------------------------
; Get Property (8 bits) using HL / Object in HL
;---------------------------------------------------------
; Input:
;	HL			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	dest		= a, b, c, d, e, h, l
; Output:
;	HL			= pProperty
;	BC			= Property offset
;	dest		= Property value
;---------------------------------------------------------
	.macro GetProp8HL_HL class, prop, dest
	GetPropPtrHL_HL class, prop
	ld dest', (HL)
	.endm

;---------------------------------------------------------
; Get Property (8 bits) using HL if dest not A / addr in parameters
;---------------------------------------------------------
; Input:
; Params:
;	addr		= address to be fetched from
;	dest		= a, b, c, d, e, h, l
; Output:
;	dest		= data
;---------------------------------------------------------
	.macro GetPropDirect addr, dest
	.if ''dest - 'a
	.if ''dest - 'A
	ld HL, #addr
	ld dest', (HL)
	.mexit
	.endif
	.endif
	ld A, (#addr)
	.endm

;---------------------------------------------------------
; Set Property (8 bits) / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	value		= a, b, c, d, e, h, l, *
; Output:
;	Property	= value
;---------------------------------------------------------
	.macro SetProp8_IY class, prop, value
	ld class'.'prop'.offset(IY), value
	.endm

;---------------------------------------------------------
; Set Property (8 bits) / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	class		= Class name
;	prop		= Property name
;	value		= a, b, c, d, e, h, l, *
; Output:
;	IY			= Object
;	Property	= value
;---------------------------------------------------------
	.macro SetProp8 object, class, prop, value
	Zoo.SetObj ^/object/
	SetProp8_IY class, prop, value
	.endm

;---------------------------------------------------------
; Set Property (8 bits) using HL / Object in HL
;---------------------------------------------------------
; Input:
;	HL			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	value		= a, d, e, *
; Output:
;	HL			= pProperty
;	BC			= Property offset
;	Property	= value
;---------------------------------------------------------
	.macro SetProp8HL_HL class, prop, value
	GetPropPtrHL_HL class, prop
	ld (HL), value
	.endm

;---------------------------------------------------------
; Set Property (8 bits) using HL / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	prop		= Property name
;	value		= a, d, e, *
; Output:
;	HL			= pProperty
;	BC			= Property offset
;	Property	= value
;---------------------------------------------------------
	.macro SetProp8HL object, prop, value
	GetPropPtrHL object, prop
	ld (HL), value
	.endm
	
;---------------------------------------------------------
; Get Property (16 bits) / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	destXXX		= a, b, c, d, e, h, l
; Output:
;	destXXX		= Property value
;---------------------------------------------------------
	.macro GetProp16_IY class, prop, destMSB, destLSB
	ld destLSB, class'.'prop'.offset(IY)
	ld destMSB, class'.'prop'.offset+1(IY)
	.endm

;---------------------------------------------------------
; Get Property MSB (16 bits) / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	dest		= a, b, c, d, e, h, l
; Output:
;	dest		= Property value MSB
;---------------------------------------------------------
	.macro GetProp16MSB_IY class, prop, dest
	ld dest, class'.'prop'.offset+1(IY)
	.endm

;---------------------------------------------------------
; Get Property LSB (16 bits) / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	dest		= a, b, c, d, e, h, l
; Output:
;	dest		= Property value LSB
;---------------------------------------------------------
	.macro GetProp16LSB_IY class, prop, dest
	ld dest, class'.'prop'.offset(IY)
	.endm

;---------------------------------------------------------
; Get Property (16 bits) / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	class		= Class name
;	prop		= Property name
;	destXXX		= a, b, c, d, e, h, l
; Output:
;	destXXX		= Property value
;---------------------------------------------------------
	.macro GetProp16 object, class, prop, destMSB, destLSB
	Zoo.SetObj ^/object/
	GetProp16_IY class, prop, destMSB, destLSB
	.endm

;---------------------------------------------------------
; Get Property (16 bits) using HL / Object in HL
;---------------------------------------------------------
; Input:
;	HL			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	destMSB		= a, b, c, d, e, h, l
;	destLSB		= a, b, c, d, e
; Output:
;	HL			= pProperty + 1
;	BC			= Property offset
;	dest		= Property value
;---------------------------------------------------------
	.macro GetProp16HL_HL class, prop, destMSB, destLSB
	GetPropPtrHL_HL class, prop
	ld destLSB, (HL)
	inc HL
	ld destMSB, (HL)
	.endm

;---------------------------------------------------------
; Get Property (16 bits) using HL / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	prop		= Property name
;	destMSB		= a, b, c, d, e, h, l
;	destLSB		= a, b, c, d, e
; Output:
;	HL			= pProperty + 1
;	BC			= Property offset
;	dest		= Property value
;---------------------------------------------------------
	.macro GetProp16HL object, prop, destMSB, destLSB
	GetPropPtrHL object, prop
	ld destLSB, (HL)
	inc HL
	ld destMSB, (HL)
	.endm

;---------------------------------------------------------
; Set Property (16 bits) / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	valueMSB	= a, b, c, d, e, h, l, *
;	valueLSB	= a, b, c, d, e, h, l, *
; Output:
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16_IY class, prop, valueMSB, valueLSB
	ld class'.'prop'.offset(IY), valueLSB
	ld class'.'prop'.offset+1(IY), valueMSB
	.endm

;---------------------------------------------------------
; Set Property (16 bits) / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	class		= Class name
;	prop		= Property name
;	valueMSB	= a, b, c, d, e, h, l, *
;	valueLSB	= a, b, c, d, e, h, l, *
; Output:
;	IY			= Object
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16 object, class, prop, valueMSB, valueLSB
	Zoo.SetObj ^/object/
	SetProp16_IY class, prop, valueMSB, valueLSB
	.endm

;---------------------------------------------------------
; Set Property (16 bits) using HL / Object in HL
;---------------------------------------------------------
; Input:
;	HL			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	valueMSB	= a, b, c, d, e, *
;	valueLSB	= a, b, c, d, e, *
; Output:
;	HL			= pProperty + 1
;	BC			= Property offset
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16HL_HL class, prop, valueMSB, valueLSB
	GetPropPtrHL_HL class, prop
	ld (HL), valueLSB
	inc HL
	ld (HL), valueMSB
	.endm

;---------------------------------------------------------
; Set Property (16 bits) using HL / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	prop		= Property name
;	valueMSB	= a, b, c, d, e, *
;	valueLSB	= a, b, c, d, e, *
; Output:
;	HL			= pProperty + 1
;	BC			= Property offset
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16HL object, prop, valueMSB, valueLSB
	GetPropPtrHL object, prop
	ld destLSB, (HL)
	inc HL
	ld destMSB, (HL)
	.endm

;---------------------------------------------------------
; Set Property (16 bits) value / Object in IY
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	value		= *
; Output:
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16Literal_IY class, prop, value
	ld class'.'prop'.offset(IY), #<value
	ld class'.'prop'.offset+1(IY), #>value
	.endm

;---------------------------------------------------------
; Set Property (16 bits) value / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	class		= Class name
;	prop		= Property name
;	value		= *
; Output:
;	IY			= Object
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16Literal object, class, prop, value
	Zoo.SetObj ^/object/
	SetProp16Literal_IY class, prop, value
	.endm

;---------------------------------------------------------
; Set Property (16 bits) value using HL / Object in HL
;---------------------------------------------------------
; Input:
;	HL			= Object
; Params:
;	class		= Class name
;	prop		= Property name
;	value		= *
; Output:
;	HL			= pProperty + 1
;	BC			= Property offset
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16LiteralHL_HL class, prop, value
	GetPropPtrHL_HL class, prop
	ld (HL), #<value
	inc HL
	ld (HL), #>value
	.endm

;---------------------------------------------------------
; Set Property (16 bits) value using HL / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	prop		= Property name
;	value		= *
; Output:
;	HL			= pProperty + 1
;	BC			= Property offset
;	Property	= value
;---------------------------------------------------------
	.macro SetProp16LiteralHL object, prop, value
	GetPropPtrHL object, prop
	ld (HL), #<value
	inc HL
	ld (HL), #>value
	.endm