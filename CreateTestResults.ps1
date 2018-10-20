Write-Host "Remember to build the solution first!"

# NUnit2
Start-Process -FilePath "$PSScriptRoot\test-harness\packages\NUnit.Runners.2.7.0\tools\nunit-console.exe" -ArgumentList "$PSScriptRoot\test-harness\nunit\bin\Debug\nunitHarness.dll", "/result=$PSScriptRoot\results-example-nunit.xml" -NoNewWindow -Wait

# NUnit 2 with NUnit3 runner 
Start-Process -FilePath "$PSScriptRoot\test-harness\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe" -ArgumentList "$PSScriptRoot\test-harness\nunit\bin\Debug\nunitHarness.dll", "/result=$PSScriptRoot\results-example-nunit2-with-nunit3-runner.xml" -NoNewWindow -Wait

# NUnit 3
Start-Process -FilePath "$PSScriptRoot\test-harness\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe" -ArgumentList "$PSScriptRoot\test-harness\nunit3\bin\Debug\nunit3Harness.dll", "/result=$PSScriptRoot\results-example-nunit3.xml" -NoNewWindow -Wait

# SpecRun
Start-Process -FilePath "$PSScriptRoot\test-harness\packages\SpecRun.Runner.1.2.0\tools\specrun.exe" -ArgumentList "run default.srprofile", "/baseFolder:$PSScriptRoot\test-harness\SpecRun\bin\Debug", "/log:specrun.log", "/report:$PSScriptRoot\results-example-specrun.html" -NoNewWindow -Wait

# XUnit 1
Start-Process -FilePath "$PSScriptRoot\test-harness\packagesNonNuget\xunit.runner\xunit.console.clr4.exe" -ArgumentList "$PSScriptRoot\test-harness\xunit\bin\Debug\xunitHarness.dll", "/xml $PSScriptRoot\results-example-xunit.xml" -NoNewWindow -Wait

# XUnit 2
Start-Process -FilePath "$PSScriptRoot\test-harness\packages\xunit.runner.console.2.4.0\tools\net452\xunit.console.exe" -ArgumentList "$PSScriptRoot\test-harness\xunit2\bin\Debug\xunit2Harness.dll", "-xml $PSScriptRoot\results-example-xunit2.xml", "-parallel none" -NoNewWindow -Wait

# MSTest
$MSTest = Get-ChildItem -Path "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017" -Recurse | Where-Object { $_.Name -eq "MSTest.exe" } | Select-Object -First 1
$MsTestResultFilePath = "$PSScriptRoot\results-example-mstest.trx"
if (Test-Path $MsTestResultFilePath) {
    Remove-Item $MsTestResultFilePath
}

Start-Process -FilePath $MSTest.FullName -ArgumentList "/testcontainer:$PSScriptRoot\test-harness\mstest\bin\Debug\mstestHarness.dll", "/resultsfile:$MsTestResultFilePath", "/testsettings:$PSScriptRoot\test-harness\TestSettings.testsettings" -NoNewWindow -Wait

# Cucumber
Set-Location -Path "$PSScriptRoot\test-harness\Cucumber"
& "cucumber" --format json_pretty --out "$PSScriptRoot\results-example-json.json" --tags ~@ignore
Set-Location $PSScriptRoot

# CucumberJS
Set-Location -Path "$PSScriptRoot\test-harness\CucumberJS"
& "$PSScriptRoot\node_modules\.bin\cucumber-js" --format json:"$PSScriptRoot\results-example-cucumberjs-json.json"
Write-Host "Waiting for CucumberJS to finish. You might need to increase the sleep time as the test suite increases."
Start-Sleep -s 2
Set-Location $PSScriptRoot

# VSTest
## Get VSTest console runner location
$VSTest = Get-ChildItem -Path "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017" -Recurse | Where-Object { $_.Name -eq "vstest.console.exe" } | Select-Object -First 1

## Remove any previous .trx files
Get-ChildItem -Path "$PSScriptRoot\TestResults" | Where-Object { $_.Extension -eq ".trx" } | Remove-Item

Start-Process -FilePath $VSTest.FullName -ArgumentList "$PSScriptRoot\test-harness\mstest\bin\Debug\mstestHarness.dll", "/logger:trx" -NoNewWindow -Wait

## Get the .trx file
$VsTestResultFile = Get-ChildItem -Path "$PSScriptRoot\TestResults" | Where-Object {$_.Extension -eq ".trx" } | Sort-Object CreationTime -Descending | Select-Object -First 1

# Moving result files to corresponding unit test projects
Write-Host "Moving NUnit 2 results"
Move-Item -Path "$PSScriptRoot\results-example-nunit.xml" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\NUnit\NUnit2\" -Force

Write-Host "Moving NUnit 2 with NUnit3 runner results"
Move-Item -Path "$PSScriptRoot\results-example-nunit2-with-nunit3-runner.xml" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\NUnit\NUnit3\" -Force

Write-Host "Moving NUnit 3 results"
Move-Item -Path "$PSScriptRoot\results-example-nunit3.xml" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\NUnit\NUnit3\" -Force

Write-Host "Moving XUnit 1 results"
Move-Item -Path "$PSScriptRoot\results-example-xunit.xml" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\XUnit\XUnit1\" -Force

Write-Host "Moving XUnit 2 results"
Move-Item -Path "$PSScriptRoot\results-example-xunit2.xml" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\XUnit\XUnit2\" -Force

Write-Host "Moving SpecRun results"
Move-Item -Path "$PSScriptRoot\results-example-specrun.html" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\SpecRun\" -Force

Write-Host "Moving Cucumber JSON results"
Move-Item -Path "$PSScriptRoot\results-example-json.json" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\CucumberJSON\" -Force

Write-Host "Moving MSTest results"
Move-Item -Path "$PSScriptRoot\results-example-mstest.trx" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\MsTest\" -Force

Write-Host "Moving CucumberJS JSON results"
Write-Host "The tags do not seem to work - remember to manually remove the ignored scenarios from the result"
Move-Item -Path "$PSScriptRoot\results-example-cucumberjs-json.json" -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\CucumberJSON\" -Force

Write-Host "Moving VSTest results"
Move-Item -Path $VsTestResultFile.FullName -Destination "$PSScriptRoot\src\Pickles\Pickles.TestFrameworks.UnitTests\VsTest\results-example-vstest.trx" -Force