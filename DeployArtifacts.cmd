@ECHO OFF
call DeployToChocolatey.cmd %1
call DeployToNuget.cmd %1
