@echo off
echo //------------------------------------------------	>  TargetConfig.h
echo //TargetConfig.h created automatically by makefile	>> TargetConfig.h
echo //------------------------------------------------	>> TargetConfig.h
echo.													>> TargetConfig.h
echo #ifndef  __TARGETCONFIG_H__						>> TargetConfig.h
echo #define  __TARGETCONFIG_H__						>> TargetConfig.h
echo.													>> TargetConfig.h

echo ;------------------------------------------------	>  TargetConfig.s
echo ;TargetConfig.s created automatically by makefile	>> TargetConfig.s
echo ;------------------------------------------------	>> TargetConfig.s
echo.													>> TargetConfig.s

for /F "tokens=1,2" %%A in  (TargetConfig_%1.txt) do  (
	if /I "%%B"=="_off" (
	echo //#define %%A 		>> TargetConfig.h
	echo %%A = 0			>> TargetConfig.s
	) else if /I "%%B"=="_on" (
	echo #define %%A 		>> TargetConfig.h
	echo %%A = 1			>> TargetConfig.s
	) else if /I "%%B"=="" (
	echo #define %%A 		>> TargetConfig.h
	echo %%A = 1			>> TargetConfig.s
	) else (
	echo #define %%A %%B	>> TargetConfig.h
	echo %%A = %%B			>> TargetConfig.s
	)
)

echo.													>> TargetConfig.h
echo #endif	//  __TARGETCONFIG_H__						>> TargetConfig.h

@echo on
