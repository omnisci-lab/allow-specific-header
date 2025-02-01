@echo off
SET SERVICE_NAME=HeaderInjectorViaProxy
SET EXE_NAME=HeaderInjectorViaProxy.exe

SET EXE_PATH=%~dp0
SET EXE_PATH=%EXE_PATH:\Scripts\=\%
SET EXE_PATH=%EXE_PATH%%EXE_NAME%

echo Installing %SERVICE_NAME%...
sc create %SERVICE_NAME% binPath= %EXE_PATH% start= auto
sc start %SERVICE_NAME%
echo Service is installed and running!

pause