@pushd %~dp0

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "SpecRun.csproj"

@if ERRORLEVEL 1 goto end

@cd ..\packages\SpecRun.Runner.*\tools

@set profile=%1
@if "%profile%" == "" set profile=Default

SpecRun.exe run %profile%.srprofile "/baseFolder:%~dp0\bin\Debug" /log:specrun.log %2 %3 %4 %5

:end

@popd

