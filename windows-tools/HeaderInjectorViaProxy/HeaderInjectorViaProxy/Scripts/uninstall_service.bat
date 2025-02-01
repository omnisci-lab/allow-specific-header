@echo off
SET SERVICE_NAME=HeaderInjectorViaProxy

echo Stopping service...
sc stop %SERVICE_NAME%
sc delete %SERVICE_NAME%
echo Service has been removed!
pause