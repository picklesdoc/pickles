@echo off
set "picklesVersion=0.19.0"

cls


cd ".\src\Pickles\packages\NuGet.CommandLine.2.8.3\tools\"

".\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "..\..\..\..\..\packages" "-ExcludeVersion"

cd ..\..\..\..\..


"packages\FAKE\tools\Fake.exe" build.fsx --envvar version %picklesVersion%
"packages\FAKE\tools\Fake.exe" nuget.fsx --envvar version %picklesVersion%
"packages\FAKE\tools\Fake.exe" chocolatey.fsx --envvar version %picklesVersion%

call unzip.cmd %picklesVersion%

FOR %%A IN (testRunnerCmd testRunnerMsBuild testRunnerPowerShell) DO (
call %%A.cmd %picklesVersion%
if errorlevel 1 goto handleerror1orhigher
)


@ECHO all fine
goto end

:handleerror1orhigher

@ECHO Something went wrong!
goto end

:end
