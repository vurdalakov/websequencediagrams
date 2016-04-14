@echo off
setlocal enableextensions enabledelayedexpansion

set zipper=..\src\packages\7ZipCLI.9.20.0\tools\7za.exe
set source=..\bin\Release
set target=WebSequenceDiagramsDesktopEditor
set zipfile=%target%.zip
set output=output.txt

if exist %zipfile% del %zipfile%
if exist %target%\nul rd /s /q %target%

md %target%

for %%f in (WebSequenceDiagramsDesktopEditor.exe,ICSharpCode.AvalonEdit.dll,Newtonsoft.Json.dll) do (
    xcopy %source%\%%f %target% 1> %output% 2>&1
    if !errorlevel! neq 0 goto error
)

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
