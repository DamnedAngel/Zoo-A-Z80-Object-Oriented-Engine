
	.include "Debug\objs\zoo.package.asm"

	.area _DATA

	.area _CODE

_zootest::
; -------------------------------
; INPUTS:
; OUTPUTS:
; -------------------------------
	Console.Cls
	Keyboard.GetChar
	ld a,#15
	ld h,#0hDA
	ld l,h
	VDP.RColor
	ld a,#8
	VDP.RScreen
	Keyboard.GetChar

;	Object._Construct obj1
;	obj1.Constructor
;	obj1.Destructor

	Panel._Construct panel1 ^/#Control.FLAG_ACTIVE|Control.FLAG_VISIBLE/, #20, #20, #40, #90, #255, #0, #VDP9938.OPERATION_IMP
	Rectangle._Construct rect1 ^/#Control.FLAG_ACTIVE|Control.FLAG_VISIBLE/, #100, #80, #100, #70, #0h1c
;								Flags, X, Y, Width, Height, XBMP, YBMP, Direction, PaintOperation
	Bitmap._Construct bmp1 ^/#Control.FLAG_ACTIVE|Control.FLAG_VISIBLE/, #200, #30, #8, #8, #0h7fc0, #0, #0, #0, #0

	DI
;	panel1.Constructor
;	panel1.SetFlags #Control.FLAG_ACTIVE|Control.FLAG_VISIBLE
;	panel1.SetX #20
;	panel1.SetY #20
;	panel1.SetWidth #30
;	panel1.SetHeight #40
;	panel1.SetColor #255
;	panel1.SetDirection #0
	panel1.Paint

;	rect1.Constructor
;	rect1.SetFlags #Control.FLAG_ACTIVE|Control.FLAG_VISIBLE
;	rect1.SetX #100
;	rect1.SetY #80
;	rect1.SetWidth #100
;	rect1.SetHeight #20
;	rect1.SetColor #9
	rect1.Paint

;	bmp1.Constructor
;	bmp1.SetFlags #Control.FLAG_ACTIVE|Control.FLAG_VISIBLE
;	bmp1.SetX #100
;	bmp1.SetY #80
;	bmp1.SetWidth #3
;	bmp1.SetHeight #3
;	bmp1.SetColor #3
;	bmp1._SetIY
	bmp1.Paint
	EI

	Keyboard.GetChar

	bmp1.SetFlags #Control.FLAG_ACTIVE|Control.FLAG_VISIBLE|Bitmap.TransparentFlag
	bmp1.SetX #180
	
	DI
	bmp1.Paint
	EI

	Keyboard.GetChar

	ld a,#15
	ld h,#4
	ld l,h
	VDP.RColor
	xor a
	VDP.RScreen
	ret
