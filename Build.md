#Development Environment

Install the dotnet-nugetize tool to assist with debugging packaging issues

```
dotnet tool install -g dotnet-nugetize
```

#Build

set version number in _build\Build.cs

PowerShell

- To execute all build targets use the following commands:

set-executionpolicy -scope process -executionpolicy bypass
.\build.ps1

- To only run a specific target and it's dependencies include the target name after the build command:

set-executionpolicy -scope process -executionpolicy bypass
.\build.ps1 -target publish

Build Targets
- Clean
- Test
- Publish
- GenerateSampleOutput
- Pack
- PublishNuGet


#Test

dotnet tool install -g --add-source <path to .nupkg without file name> <file name without version>


#Docker

-Dockerfile
--tools
---Pickles.CommandLine.[versionnumber].nupkg

To install from the root folder/docker context (after copying it into the context):
dotnet tool install --global --add-source ./tools Pickles.CommandLine --version [versionnumber]

