	.include "zoo.reflection.r0_none.asm"

Class.InheritanceReflection.offset			.equ Class.NoInheritanceReflectionEnd.offset

Class.pParentClass.offset					.equ Class.InheritanceReflection.offset
Class.pSProperties.offset					.equ Class.pParentClass.offset + 2
Class.pSMethods.offset						.equ Class.pSProperties.offset + 2
Class.ObjectSize.offset						.equ Class.pSMethods.offset + 2

Class.InheritReflectionEnd.offset			.equ Class.ObjectSize.offset + 1
