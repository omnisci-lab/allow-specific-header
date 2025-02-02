@echo off
setlocal

:: Set mitmweb.exe as the default program
set apppath="C:\Program Files\mitmproxy\bin"
set tool=mitmweb.exe
set args=-s "%~dp0add_header.py"

:: Check if Windows Terminal (wt.exe) is available
where wt >nul 2>nul
if %errorlevel%==0 (
    start wt powershell -Command "& '%apppath%\%tool%' %args%"
) else (
    start powershell -Command "& '%apppath%\%tool%' %args%"
)

pause
endlocal