@echo off
setlocal enableextensions enabledelayedexpansion

set zipper=..\src\packages\7ZipCLI.9.20.0\tools\7za.exe
set source=..\bin\Release
set target=WebSequenceDiagramsDesktopEditor
set output=output.txt

if exist %target%\nul rd /s /q %target%

md %target%

for %%f in (WebSequenceDiagramsDesktopEditor.exe,ICSharpCode.AvalonEdit.dll,Newtonsoft.Json.dll,LibGit2Sharp.dll) do (
    xcopy %source%\%%f %target% 1> %output% 2>&1
    if !errorlevel! neq 0 goto error
)

for %%f in (amd64,x86) do (
    md %target%\NativeBinaries\%%f
    xcopy %source%\NativeBinaries\%%f\git2*.dll %target%\NativeBinaries\%%f 1> %output% 2>&1
    if !errorlevel! neq 0 goto error
)

set exefile=%source%\%target%.exe

reg.exe ADD "HKCU\Software\Sysinternals\Sigcheck" /v EulaAccepted /t REG_DWORD /d 1 /f >nul
for /f "delims=. tokens=1,2" %%a in ('tools\sigcheck.exe -q -n %exefile%') do (
    set v1=%%a
    set v2=%%b
    )

set v2=0%v2%
set v2=%v2:~-2%
set zipfile=%target%_%v1%_%v2%.zip
    
if exist %zipfile% del %zipfile%
    
%zipper% a -tzip %zipfile% %target% 1> %output% 2>&1
if %errorlevel% neq 0 goto error
    
%zipper% t %zipfile% 1> %output% 2>&1
if %errorlevel% neq 0 goto error

if exist %output% del %output%
if exist %target%\nul rd /s /q %target%

echo ZIP OK: %zipfile%

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
