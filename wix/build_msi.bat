@echo off

set wixpath=..\..\src\packages\WiX.3.10.2\tools
set candle="%wixpath%\candle.exe"
set light="%wixpath%\light.exe"
set replace=freplace.exe
set output=output.txt

set filename=WebSequenceDiagramsDesktopEditor

set wxs=%filename%.wxs
set wixobj=%filename%.wixobj

set source=..\..\bin\Release
set exefile=%source%\%filename%.exe

set builddir=build
if exist %builddir%\. rmdir /s /q %builddir%

mkdir %builddir% 1> %output% 2>&1
if %errorlevel% neq 0 goto error

xcopy files\*.* %builddir% 1> %output% 2>&1
if %errorlevel% neq 0 goto error

xcopy tools\*.* %builddir% 1> %output% 2>&1
if %errorlevel% neq 0 goto error

xcopy %wxs% %builddir% 1> %output% 2>&1
if %errorlevel% neq 0 goto error

cd %builddir%

reg.exe ADD "HKCU\Software\Sysinternals\Sigcheck" /v EulaAccepted /t REG_DWORD /d 1 /f >nul
for /f "delims=. tokens=1,2" %%a in ('sigcheck.exe -q -n %exefile%') do (
    set version=%%a.%%b
    set msi=%filename%_%%a_%%b.msi
    )

if exist ..\%msi% del ..\%msi%

%replace% %wxs% "ApplicationVersion" "%version%" 1> %output% 2>&1
if %errorlevel% neq 0 goto error

%candle% %wxs% -ext WiXNetFxExtension -ext WixUtilExtension 1> %output% 2>&1
if %errorlevel% neq 0 goto error

if not exist %wixobj% (
echo %wixobj% not created > %output%
set errorlevel=1
goto error
)

%light% %wixobj% -b %source% -o %msi% -ext WixUIExtension -ext WiXNetFxExtension -ext WixUtilExtension -sice:ICE91 -sice:ICE38 1> %output% 2>&1
if %errorlevel% neq 0 goto error

if not exist %msi% (
echo %msi% not created > %output%
set errorlevel=1
goto error
)

xcopy %msi% .. 1> %output% 2>&1
if %errorlevel% neq 0 goto error

cd ..

if exist %output% del %output%
if exist %builddir%\nul rd /s /q %builddir%

echo MSI OK: %msi%

exit /b 0

:error

set error=%errorlevel%
echo.
echo -----------------------------------------------------
echo --- ERROR (errorlevel %error%)
type %output%
echo -----------------------------------------------------
if exist %output% del %output%
exit /b %error%
