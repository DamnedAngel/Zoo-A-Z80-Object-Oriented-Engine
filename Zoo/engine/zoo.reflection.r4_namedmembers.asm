	.include "zoo.reflection.r3_classstructure.asm"

Class.NamedMembersReflection.offset			.equ Class.ClassStructureReflectionEnd.offset

Class.PropertiesCount.offset				.equ Class.NamedInheritanceReflection.offset
Class.MethodsCount.offset					.equ Class.PropertiesCount.offset + 1
Class.SPropertiesCount.offset				.equ Class.MethodsCount.offset + 1
Class.SMethodsCount.offset					.equ Class.SPropertiesCount.offset + 1
Class.pPropertiesNames.offset				.equ Class.SMethodsCount.offset + 1
Class.pSPropertiesNames.offset				.equ Class.pPropertiesNames.offset + 2

Class.NamedMembersReflectionEnd.offset		.equ Class.pSPropertiesNames.offset + 2

