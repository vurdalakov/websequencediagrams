@echo off

call build_zip.bat
if %errorlevel% neq 0 exit %errorlevel%

call build_msi.bat
if %errorlevel% neq 0 exit %errorlevel%

echo All OK
