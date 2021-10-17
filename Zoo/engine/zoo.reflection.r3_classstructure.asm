	.include "zoo.reflection.r2_namedinheritance.asm"

Class.ClassStructureReflection.offset		.equ Class.NamedInheritanceReflectionEnd.offset

Class.pSisterClass.offset					.equ Class.ClassStructureReflection.offset
Class.pChildChain.offset					.equ Class.pSisterClass.offset + 2

Class.ClassStructureReflectionEnd.offset	.equ Class.pChildChain.offset + 2
