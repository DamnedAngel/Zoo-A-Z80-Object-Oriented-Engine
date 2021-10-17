;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Properties Pointers Macros
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
; Get Pointer to Property in HL / Object in HL
;---------------------------------------------------------
; Input:
;	HL			= Object
; Params:
;	class		= Class name
;	prop		= Property name
; Output:
;	HL			= pProperty
;	BC			= Property offset
;---------------------------------------------------------
	.macro GetPropPtrHL_HL class, prop
	ld BC, #class'.'prop'.offset
	add HL, BC
	.endm

;---------------------------------------------------------
; Get Pointer to Property in HL / Object in IY
; WARNING! THE OUTPUT OF THIS MACRO IS DIFFERENT FROM
; THE OTHER GETPROPPTR MACROS!
;---------------------------------------------------------
; Input:
;	IY			= Object
; Params:
;	class		= Class name
;	prop		= Property name
; Output:
;	HL			= pProperty
;	BC			= pObject
;---------------------------------------------------------
	.macro GetPropPtrHL_IY class, prop
	ld_b_iyh
	ld_c_iyl
	ld HL, #class'.'prop'.offset
	add HL, BC
	.endm
	
;---------------------------------------------------------
; Get Pointer to Property in HL / Object in parameters
;---------------------------------------------------------
; Input:
; Params:
;	object		= Object
;	prop		= Property name
; Output:
;	HL			= pProperty
;---------------------------------------------------------
	.macro GetPropPtrHL object, prop
	ld HL, #object'.'prop
	.endm