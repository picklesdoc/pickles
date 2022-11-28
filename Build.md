#Development Environment

Install the nuke-build tool in order to run the build commands:

```
dotnet tool install Nuke.GlobalTool --global
```

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


#Test NuGet Packages Locally

- create an empty folder
- at the command line enter "dotnet new tool-manifest"
- at the command line enter "dotnet tool install --add-source <path to .nupkg without file name> <file name without version>"
