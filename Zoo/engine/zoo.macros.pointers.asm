;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Pointer Macros
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
; Set Object
;---------------------------------------------------------
; Output:
;	IY			= pObject
;---------------------------------------------------------
	.macro Zoo.SetObj object
	ld IY, #object
	.endm
	
;---------------------------------------------------------
; Set Class for object (object in IY)
;---------------------------------------------------------
; Input:
;	IY			= pObject
; Output:
;---------------------------------------------------------
	.macro Zoo.SetClass_IY class
	ld 0(IY), #<'class							; pressuposes pParentClass.offset = 0
	ld 1(IY), #>'class							; pressuposes pParentClass.offset = 0
	.endm

;---------------------------------------------------------
; Set Class for object.
;---------------------------------------------------------
; Output:
;	IY			= pObject
;---------------------------------------------------------
	.macro Zoo.SetClass object,class
	Zoo.SetObj ^/object/
	Zoo.SetClass ^/class/
	.endm

;---------------------------------------------------------
; Get Object Class (object in IY)
;---------------------------------------------------------
; Input:
;	IY			= pObject
; Output:
;	HiReg,LoReg = pClass
;---------------------------------------------------------
	.macro Zoo.GetClass_IY HiReg, LoReg
	ld LoReg, 0(IY)								; pressuposes pParentClass.offset = 0
	ld HiReg, 1(IY)								; pressuposes pParentClass.offset = 0
	.endm

;---------------------------------------------------------
; Get Object Class
;---------------------------------------------------------
; Input:
; Output:
;	IY			= pObject
;	HiReg,LoReg = pClass
;---------------------------------------------------------
	.macro Zoo.GetClass object
	Zoo.SetObj ^/object/
	Zoo.GetClass HiReg, LoReg
	.endm
