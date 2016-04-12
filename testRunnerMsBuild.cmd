"\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" testOutput.proj /p:Version=%1 /p:ShouldIncludeExperimentalFeatures=false
"\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" testOutput.proj /p:Version=%1 /p:ShouldIncludeExperimentalFeatures=true
if errorlevel 1 exit /b 1
exit /b 0
