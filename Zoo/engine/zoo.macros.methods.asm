;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Method Macros
;---------------------------------------------------------
;---------------------------------------------------------

;---------------------------------------------------------
; Call Method (object in IY)
;---------------------------------------------------------
; Call method for object IY, considering polymorphism.
; The class parameter is the base class for polymorphism.
; If a direct call to the method of a specific class is intended,
; please use "call <class>._<method>" directly.
; IY, HL and BC cannot be used to transfer parameters.
;---------------------------------------------------------
; Input:
;	IY			= pObject
; Output:
;---------------------------------------------------------
	.macro Zoo.CallMethod_IY class, method
	Zoo.GetClass_IY H, L
	ld bc, #class'._'method'.offset
	add hl, bc
	ld c, (hl)
	inc hl
	ld h, (hl)
	ld l,c
	call _zoo_call_hl
	.endm

;---------------------------------------------------------
; Init Object (object in IY)
;---------------------------------------------------------
; Calls Constructor for the class-defined object in IY.
; IY, HL and BC cannot be used to transfer parameters.
;---------------------------------------------------------
; Input:
;	IY			= Class-defined pObject
; Output:
;---------------------------------------------------------
	.macro Zoo.Init_IY
	Zoo.CallMethod_IY Object, Constructor
	.endm

;---------------------------------------------------------
; Init Object
;---------------------------------------------------------
; Calls a class-defined object's Constructor.
; IY, HL and BC cannot be used to transfer parameters.
;---------------------------------------------------------
; Input:
; Output:
;	IY			= pObject
;---------------------------------------------------------
	.macro Zoo.Init object
	Zoo.CallMethod ^/object/, Object, Constructor
	.endm

;---------------------------------------------------------
; Destroy Object (Object in IY)
;---------------------------------------------------------
; Calls destructor of object in IY.
;---------------------------------------------------------
; Input:
;	IY			= pObject
; Output:
;---------------------------------------------------------
	.macro Zoo.Destroy_IY
	Zoo.CallMethod_IY Object,Destructor
	.endm

;---------------------------------------------------------
; Destroy Object
;---------------------------------------------------------
; Calls destructor of object.
;---------------------------------------------------------
; Input:
; Output:
;---------------------------------------------------------
	.macro Zoo.Destroy object
	Zoo.CallMethod ^/object/, Object, Destructor
	.endm