@echo off
SET SERVICE_NAME=HeaderInjectorViaProxy
SET EXE_NAME=HeaderInjectorViaProxy.WinServices.exe

SET EXE_PATH=%~dp0
SET EXE_PATH=%EXE_PATH:\Scripts\=\%
SET EXE_PATH=%EXE_PATH%%EXE_NAME%

echo Stopping service...
sc stop %SERVICE_NAME%
sc delete %SERVICE_NAME%
echo Service has been removed!

echo Installing %SERVICE_NAME%...
sc create %SERVICE_NAME% binPath= %EXE_PATH% start= auto
sc start %SERVICE_NAME%
echo Service is installed and running!

pause