;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Property Accessor Macros for Classes
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
;- 8 Bits
;---------------------------------------------------------
	.macro Property8BitsAccessorsForClasses class, property
	
	.macro class'.Get'property'_IY dest
	GetProp8_IY class, property, dest
	.endm
	
	.macro class'.Get'property object, dest
	GetProp8 object, class, property, dest
	.endm

	.macro class'.Get'property'HL_HL dest
	GetProp8HL_HL class, property, dest
	.endm

	.macro class'.Get'property'HL object, dest
	GetProp8HL object, property, dest
	.endm

	.macro class'.Set'property'_IY value
	SetProp8_IY class, property, value
	.endm

	.macro class'.Set'property object, value
	SetProp8 object, class, property, value
	.endm

	.macro class'.Set'property'HL_HL value
	SetProp8HL_HL class, property, value
	.endm

	.macro class'.Set'property'HL object, value
	SetProp8HL object, property, value
	.endm
	
	.endm


;---------------------------------------------------------
;- 16 Bits
;---------------------------------------------------------
	.macro Property16BitsAccessorsForClasses class, property

	.macro class'.Get'property'_IY destMSB, destLSB
	GetProp16_IY class, property, destMSB, destLSB
	.endm

	.macro class'.Get'property'MSB_IY dest
	GetProp16MSB_IY class, property, dest
	.endm
	
	.macro class'.Get'property'LSB_IY dest
	GetProp16LSB_IY class, property, dest
	.endm
	
	.macro class'.Get'property object, destMSB, destLSB
	GetProp16 object, class, property, destMSB, destLSB
	.endm

	.macro class'.Get'property'HL_HL destMSB, destLSB
	GetProp16HL_HL class, property, destMSB, destLSB
	.endm

	.macro class'.Get'property'HL object, destMSB, destLSB
	GetProp16HL object, property, destMSB, destLSB
	.endm

	.macro class'.Set'property'_IY valueMSB, valueLSB
	SetProp16_IY class, property, valueMSB, valueLSB
	.endm

	.macro class'.Set'property object, valueMSB, valueLSB
	SetProp16 object, class, property, valueMSB, valueLSB
	.endm

	.macro class'.Set'property'HL_HL valueMSB, valueLSB
	SetProp16HL_HL class, property, valueMSB, valueLSB
	.endm

	.macro class'.Set'property'HL object, valueMSB, valueLSB
	SetProp16HL object, property, valueMSB, valueLSB
	.endm

	.macro class'.Set'property'Literal_IY value
	SetProp16Literal_IY class, property, value
	.endm

	.macro class'.Set'property'Literal object, value
	SetProp16Literal object, class, property, value
	.endm

	.macro class'.Set'property'LiteralHL_HL value
	SetProp16LiteralHL_HL class, property, value
	.endm

	.macro class'.Set'property'LiteralHL object, value
	SetProp16LiteralHL object, property, value
	.endm
			
	.endm


;---------------------------------------------------------
;- Pointers
;---------------------------------------------------------
	.macro PropertyPointersAccessorsForClasses class, property

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
