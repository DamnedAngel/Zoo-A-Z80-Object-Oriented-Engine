;---------------------------------------------------------
;---------------------------------------------------------
;- Zoo Method Accessor Macros for Objects
;---------------------------------------------------------
;---------------------------------------------------------

	.macro MethodAccessorsForObjects class, object, method

	.macro	object'.'method
	Zoo.SetObj object
	call class'._'method
	.endm


	.endm
