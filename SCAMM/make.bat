;@echo off

set MSX_DEV_PATH=..\..\..
set MSX_LIB_PATH=%MSX_DEV_PATH%\libs
set MSX_TOOLS_PATH=%MSX_DEV_PATH%\tools
set PRJ_OBJ_PATH=%1\objs
set OBJLIST=
set ZOOLIST=
set INCDIRS=
SET LIBDIR=%MSX_LIB_PATH%\fusion-c\lib

SETLOCAL ENABLEDELAYEDEXPANSION

IF NOT EXIST %1 mkdir %1
IF NOT EXIST %1\objs mkdir %1\objs
IF NOT EXIST %1\bin mkdir %1\bin

echo -----------------------------------------------------------------------------------
echo MSX SDCC MAKEFILE by Danilo Angelo
if /I not "%3"=="clean" GOTO TARGETCONFIGURATION
echo -----------------------------------------------------------------------------------
echo Cleaning...
IF EXIST *.rel del *.rel /F /Q
IF EXIST %1\objs\*.* del %1\objs\*.* /F /Q
IF EXIST %1\bin\%2.com del %1\bin\%2.com
echo Done.

if /I not "%4"=="all" GOTO END

:TARGETCONFIGURATION
echo -----------------------------------------------------------------------------------
echo Building Target configuration defines...
set MSX_BUILD_TIME=%TIME% 
set MSX_BUILD_DATE=%DATE% 
echo //-------------------------------------------------	>  TargetConfig.h
echo // TargetConfig.h created automatically by makefile	>> TargetConfig.h
echo // on %MSX_BUILD_TIME%, %MSX_BUILD_DATE%				>> TargetConfig.h
echo //-------------------------------------------------	>> TargetConfig.h
echo.														>> TargetConfig.h
echo #ifndef  __TARGETCONFIG_H__							>> TargetConfig.h
echo #define  __TARGETCONFIG_H__							>> TargetConfig.h
echo.														>> TargetConfig.h

echo ;-------------------------------------------------		>  TargetConfig.s
echo ; TargetConfig.s created automatically by makefile		>> TargetConfig.s
echo ; on %MSX_BUILD_TIME%, %MSX_BUILD_DATE%				>> TargetConfig.s
echo ;-------------------------------------------------		>> TargetConfig.s
echo.														>> TargetConfig.s

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

echo.														>> TargetConfig.h
echo #endif	//  __TARGETCONFIG_H__							>> TargetConfig.h
echo Done.

if "%3"=="" GOTO COMPILE
if /I "%3"=="all" GOTO ALL
if /I not "%4"=="all" GOTO END

:ALL
echo -----------------------------------------------------------------------------------
echo Compiling Zoo Package Source...
for /F "tokens=1" %%A in  (ZooPackage.txt) do  (
	set ZOOFILE=%%A
	if NOT "%ZOOFILE:~0,1%"==";" (
		set ZOOFILE=!ZOOFILE:[MSX_LIB_PATH]=%MSX_LIB_PATH%!
		set ZOOFILE=!ZOOFILE:[PRJ_OBJ_PATH]=%PRJ_OBJ_PATH%!
		set ZOOLIST=!ZOOLIST! !ZOOFILE!
	)
)
if  "%ZOOLIST%"=="" (
	echo No Zoo Package.
) else (
	call %MSX_TOOLS_PATH%\ZooBuilder\ZooBuilder.exe -v -d %1\objs  -i %ZOOLIST% -z %MSX_LIB_PATH%\zoo -r inheritance
	if !errorlevel! NEQ 0 (
		echo FAIL!	
		echo Compiling Zoo Package Source!
		EXIT !errorlevel!
	)
	echo sdasz80 -I%MSX_LIB_PATH%\zoo\engine -I%MSX_LIB_PATH%\z80 -ols %1\objs\zoo.package.s
	sdasz80 -I%MSX_LIB_PATH%\zoo\engine -I%MSX_LIB_PATH%\z80 -ols %1\objs\zoo.package.s
	if !errorlevel! NEQ 0 (
		echo FAIL!	
		echo Fail building zoo.package.s!
		EXIT !errorlevel!
	)
	set OBJLIST=!OBJLIST! %1\objs\zoo.package.rel
	echo Done.
)

echo -----------------------------------------------------------------------------------
echo Building libraries...
for /F "tokens=*" %%A in (LibrarySources.txt) do (
	set LIBFILE=%%A
	if NOT "%LIBFILE:~0,1%"==";" (
		set LIBFILE=!LIBFILE:[MSX_LIB_PATH]=%MSX_LIB_PATH%!
		set LIBFILE=!LIBFILE:[PRJ_OBJ_PATH]=%PRJ_OBJ_PATH%!
		set RELFILE=%1\objs\%%~nA.rel
		if /I "%%~xA"==".c" (
			<NUL set /p=Processing C file !LIBFILE!... 
			sdcc -mz80 -c -o !RELFILE! !LIBFILE!
		) else (
			<NUL set /p=Processing ASM file !LIBFILE!... 
			sdasz80 -I%MSX_LIB_PATH%\zoo\engine -I%MSX_LIB_PATH%\z80 -o !RELFILE! !LIBFILE!
		)
		if !errorlevel! NEQ 0 (
			echo FAIL!
			echo Failed building %%A!
			EXIT !errorlevel!
		)
		echo Done.
	)
)

:COMPILE

for /F "tokens=*" %%A in (LibrarySources.txt) do (
	set LIBFILE=%%A
	if NOT "%LIBFILE:~0,1%"==";" (
		set LIBFILE=!LIBFILE:[MSX_LIB_PATH]=%MSX_LIB_PATH%!
		set LIBFILE=!LIBFILE:[PRJ_OBJ_PATH]=%PRJ_OBJ_PATH%!
		set RELFILE=%1\objs\%%~nA.rel
		set OBJLIST=!OBJLIST! !RELFILE!
	)
)

for /F "tokens=*" %%A in (RELs.txt) do (
	set RELFILE=%%A
	if NOT "%RELFILE:~0,1%"==";" (
		set RELFILE=!RELFILE:[MSX_LIB_PATH]=%MSX_LIB_PATH%!
		set RELFILE=!RELFILE:[PRJ_OBJ_PATH]=%PRJ_OBJ_PATH%!
		set OBJLIST=!OBJLIST! !RELFILE!
	)
)

echo -----------------------------------------------------------------------------------
echo Building assembler modules...
for /F "tokens=1" %%A in  (AssemblerSources.txt) do  (
	set ASMFILE=%%A
	if NOT "%ASMFILE:~0,1%"==";" (
		set ASMFILE=!ASMFILE:[MSX_LIB_PATH]=%MSX_LIB_PATH%!
		set ASMFILE=!ASMFILE:[PRJ_OBJ_PATH]=%PRJ_OBJ_PATH%!
		set RELFILE=%1\objs\%%~nA.rel
		<NUL set /p=Processing ASM file !ASMFILE!... 
		echo sdasz80 -I%MSX_LIB_PATH%\zoo\engine -I%MSX_LIB_PATH%\z80 -lo !RELFILE! !ASMFILE!
		sdasz80 -I%MSX_LIB_PATH%\zoo\engine -I%MSX_LIB_PATH%\z80 -lo !RELFILE! !ASMFILE!
		if !errorlevel! NEQ 0 (
			echo FAIL!	
			echo Failed building %%A!
			EXIT !errorlevel!
		)
		echo Done.
		set OBJLIST=!OBJLIST! !RELFILE!
	)
)
echo Done.

echo -----------------------------------------------------------------------------------
echo Compiling...

for /F "tokens=*" %%A in (IncludeDirectories.txt) do (
	set INCDIR=%%A
	if NOT "%INCDIR:~0,1%"==";" (
		set INCDIR=!INCDIR:[MSX_LIB_PATH]=%MSX_LIB_PATH%!
		set INCDIR=!INCDIR:[PRJ_OBJ_PATH]=%PRJ_OBJ_PATH%!
		set INCDIRS=!INCDIRS! -I!INCDIR!
	)
)

call %MSX_TOOLS_PATH%\msxc.bat -L %LIBDIR% %OBJLIST% %INCDIRS% #s%2 #o%1\objs\
if %errorlevel% NEQ 0 (
echo FAIL!
EXIT %errorlevel%
)
copy %1\objs\%2.com %1\bin\
echo Done.
echo -----------------------------------------------------------------------------------
echo Building Symbol file...
python %MSX_TOOLS_PATH%\symbol.p %1\objs\ %2
if %errorlevel% NEQ 0 (
echo FAIL!
EXIT %errorlevel%
)
echo Done.

:END
echo -----------------------------------------------------------------------------------
@echo on
EXIT
''