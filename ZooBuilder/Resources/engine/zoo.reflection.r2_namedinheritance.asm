	.include "zoo.reflection.r1_inheritance.asm"

Class.NamedInheritanceReflection.offset		.equ Class.InheritReflectionEnd.offset

Class.pName.offset							.equ Class.NamedInheritanceReflection.offset

Class.NamedInheritanceReflectionEnd.offset	.equ Class.pName.offset + 2
