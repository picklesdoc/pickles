"\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" testOutput.proj /p:Version=%1
if errorlevel 1 exit /b 1
exit /b 0
