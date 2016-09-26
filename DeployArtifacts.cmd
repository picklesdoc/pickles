@ECHO OFF
call DeployOutput.cmd %1
call DeployToChocolatey.cmd %1
call DeployToNuget.cmd %1
