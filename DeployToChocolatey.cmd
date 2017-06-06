@ECHO OFF

REM this script supposes you set the chocolatey API key by running this command:
REM   choco apikey -s"https://push.chocolatey.org/" -k="<your key here>"
REM Mind the trailing slash!

.\packages\chocolatey\tools\chocolateyInstall\choco push .\deploy\chocolatey\pickles.%1.nupkg
.\packages\chocolatey\tools\chocolateyInstall\choco push .\deploy\chocolatey\picklesui.%1.nupkg
