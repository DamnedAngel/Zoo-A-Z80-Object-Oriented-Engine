	.include "zoo.reflection.r3_classstructure.asm"


	.area	_DATA



	.area	_CODE

_zoo_init_object::
; -------------------------------
; INPUTS:
;	SP + 02 = pObject
;	SP + 04 = pClass
; OUTPUTS:
;	AF, BC, DE, HL, IY: Changes
; -------------------------------
	ld hl, #2
	add hl, sp
	ld de, #BIOS_FORCLR
	ld bc, #3
	ldir

_zoo_init_object_from_regs::
; -------------------------------
; INPUTS:
;	IY		= pObject
;	HL		= pClass
; OUTPUTS:
;	AF, BC, DE, HL, IY: Changes
; -------------------------------
	SetAttr16 Object.pClass.offset, H, L
	ld iy,(#BIOS_ROMSLT)
	ld ix,#BIOS_CHGCLR
	call #BIOS_CALSLT
	pop ix
	ret


_zoo_install_class::
;--------------------------------
; INPUTS:
;	IY		= pClass
; OUTPUTS:
;	BC, HL : Cganges
;--------------------------------
    GetRefProp16 h, l, pParentClass                     ; finds parent class
    ld bc, #Class.pChildChain.offset
    add hl, bc                                          ; finds Child Chain of parent class
    ld c, (hl)
    inc hl
    ld b, (hl)
    SetRefProp16 pSisterClass, b, c                     ; Sets the first subclass of the parent class as sister of current class
    ld_c_iyh
    ld (hl),c
    dec hl
    ld_c_iyl
    ld (hl),c                                           ; Sets the current class as first subclass of the parent class
    ret
  

Object.Uninstall::
	nop
Object._Uninstall::
;--------------------------------
; INPUTS:
;	IY		= pClass
; OUTPUTS:
;	A, BC, HL : Cganges
;--------------------------------
    GetRefPropPtr_HL pChildChain                        ; finds Child Chain of parent class
_Uninstall_compare:
    ld a, (hl)
    inc hl
    cp_iyl
    jr nz, _Uninstall_next
    ld a, (hl)
    cp_iyh
    jr nz, _Uninstall_next
    GetRefProp16 b, c, pSisterClass                     ; gets sister of current class
    ld (hl), b
    dec hl
    ld (hl), c
    ret
_Uninstall_next:
    ld b, (hl)
    dec hl
    ld c, (hl)
    ld h, b
    ld l, c
    jr _Uninstall_compare