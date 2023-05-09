@echo off

powershell -ExecutionPolicy bypass -File "%~dp0\build.ps1 %*"
pause
