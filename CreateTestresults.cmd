@echo off
@pushd %~dp0

ECHO Remember to build the solution first!

"%~dp0\test-harness\packages\NUnit.Runners.2.6.4\tools\nunit-console.exe" "%~dp0\test-harness\nunit\bin\Debug\nunitHarness.dll" /result="%~dp0\results-example-nunit.xml"
"%~dp0\test-harness\packages\NUnit.Console.3.0.0\tools\nunit3-console.exe" "%~dp0\test-harness\nunit\bin\Debug\nunitHarness.dll" /result="%~dp0\results-example-nunit2-with-nunit3-runner.xml"

"%~dp0\test-harness\packages\NUnit.Console.3.0.0\tools\nunit3-console.exe" "%~dp0\test-harness\nunit3\bin\Debug\nunit3Harness.dll" /result="%~dp0\results-example-nunit3.xml"

"%~dp0\test-harness\packages\SpecRun.Runner.1.2.0\tools\specrun.exe" run default.srprofile "/baseFolder:%~dp0\test-harness\SpecRun\bin\Debug" /log:specrun.log /report:"%~dp0\results-example-specrun.html"

"%~dp0\test-harness\packagesNonNuget\xunit.runner\xunit.console.clr4.exe" "%~dp0\test-harness\xunit\bin\Debug\xunitHarness.dll" /xml "%~dp0\results-example-xunit.xml"

"%~dp0\test-harness\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe" "%~dp0\test-harness\xunit2\bin\Debug\xunit2Harness.dll" -xml "%~dp0\results-example-xunit2.xml" -parallel none

del "%~dp0\results-example-mstest.trx"
"%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe" /testcontainer:"%~dp0\test-harness\mstest\bin\Debug\mstestHarness.dll" /resultsfile:"%~dp0\results-example-mstest.trx" /testsettings:"%~dp0\test-harness\TestSettings.testsettings"

cd "%~dp0\test-harness\Cucumber"
call cucumber --format json_pretty --out "%~dp0\results-example-json.json" --tags ~@ignore
cd "%~dp0"

cd "%~dp0\test-harness\CucumberJS"
call ..\..\node_modules\.bin\cucumber-js --format json:"..\..\results-example-cucumberjs-json.json" --tags ~@ignore
cd "%~dp0"

rmdir /s /q "%~dp0\TestResults\"
"%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "%~dp0\test-harness\mstest\bin\Debug\mstestHarness.dll" /logger:trx
FOR /R "%~dp0\TestResults\" %%G IN (*.trx) DO move "%%G" "%~dp0\results-example-vstest.trx"

move "%~dp0\results-example-nunit.xml" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\NUnit\NUnit2\"
move "%~dp0\results-example-nunit2-with-nunit3-runner.xml" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\NUnit\NUnit3\"
move "%~dp0\results-example-nunit3.xml" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\NUnit\NUnit3\"
move "%~dp0\results-example-xunit.xml" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\XUnit\XUnit1\"
move "%~dp0\results-example-xunit2.xml" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\XUnit\XUnit2\"
move "%~dp0\results-example-specrun.html" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\SpecRun\"
move "%~dp0\results-example-json.json" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\CucumberJSON\"
move "%~dp0\results-example-mstest.trx" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\MsTest\"
move "%~dp0\results-example-cucumberjs-json.json" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\CucumberJSON\"
move "%~dp0\results-example-vstest.trx" "%~dp0\src\Pickles\Pickles.TestFrameworks.UnitTests\VsTest\"

@popd
