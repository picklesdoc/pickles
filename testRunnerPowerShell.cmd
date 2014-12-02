powershell.exe -nologo -noprofile -command "& { .\testOutput.ps1 %1; exit $LastExitCode}"

if errorlevel 1 exit /b 1
exit /b 0
