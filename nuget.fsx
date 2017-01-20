// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake

// Properties
let deployDir  = "./deploy/nuget/"
let cmdDir = "./build/exe/"
let msBuildDir = "./build/msbuild/"
let powerShellDir = "./build/powershell/"
let packagingDir = "./packaging/"

// version info
let version = environVar "version" // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [deployDir; packagingDir; ]
)


Target "CreatePackageCommandLine" (fun _ ->
    // Copy all the package files into a package folder
    CopyFiles packagingDir [
        cmdDir + "Autofac.dll";
        cmdDir + "ClosedXML.dll";
        cmdDir + "DocumentFormat.OpenXml.dll";
        cmdDir + "Gherkin.dll";
        cmdDir + "MarkdownDeep.dll";
        cmdDir + "NDesk.Options.dll";
        cmdDir + "Newtonsoft.Json.dll";
        cmdDir + "NLog.dll";
        cmdDir + "PicklesDoc.Pickles.Library.dll";
        cmdDir + "PicklesDoc.Pickles.ObjectModel.dll";
        cmdDir + "PicklesDoc.Pickles.TestFrameworks.dll";
        cmdDir + "PicklesDoc.Pickles.DocumentationBuilders.Cucumber.dll";
        cmdDir + "PicklesDoc.Pickles.DocumentationBuilders.Word.dll";
        cmdDir + "PicklesDoc.Pickles.DocumentationBuilders.Excel.dll";
        cmdDir + "PicklesDoc.Pickles.DocumentationBuilders.Json.dll";
        cmdDir + "PicklesDoc.Pickles.DocumentationBuilders.Html.dll";
        cmdDir + "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.dll";
        cmdDir + "System.IO.Abstractions.dll";
        cmdDir + "FeatureSwitcher.dll";
        cmdDir + "Strike.Jint.dll";
        cmdDir + "Jint.dll";
        cmdDir + "pickles.exe";
        cmdDir + "NLog.config" ]

    NuGet (fun p ->
        {p with
            OutputPath = deployDir
            WorkingDir = packagingDir
            Version = version
            Publish = false })
            "src/Pickles/Pickles.CommandLine/Pickles.CommandLine.nuspec"
)

Target "CreatePackageMsBuild" (fun _ ->
    // Copy all the package files into a package folder
    CopyFiles packagingDir [
        msBuildDir + "Autofac.dll";
        msBuildDir + "ClosedXML.dll";
        msBuildDir + "DocumentFormat.OpenXml.dll";
        msBuildDir + "Gherkin.dll";
        msBuildDir + "MarkdownDeep.dll";
        msBuildDir + "NDesk.Options.dll";
        msBuildDir + "Newtonsoft.Json.dll";
        msBuildDir + "NLog.dll";
        msBuildDir + "PicklesDoc.Pickles.Library.dll";
        msBuildDir + "PicklesDoc.Pickles.ObjectModel.dll";
        msBuildDir + "PicklesDoc.Pickles.TestFrameworks.dll";
        msBuildDir + "PicklesDoc.Pickles.DocumentationBuilders.Cucumber.dll";
        msBuildDir + "PicklesDoc.Pickles.DocumentationBuilders.Word.dll";
        msBuildDir + "PicklesDoc.Pickles.DocumentationBuilders.Excel.dll";
        msBuildDir + "PicklesDoc.Pickles.DocumentationBuilders.Json.dll";
        msBuildDir + "PicklesDoc.Pickles.DocumentationBuilders.Html.dll";
        msBuildDir + "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.dll";
        msBuildDir + "System.IO.Abstractions.dll";
        msBuildDir + "FeatureSwitcher.dll";
        msBuildDir + "Strike.Jint.dll";
        msBuildDir + "Jint.dll";
        msBuildDir + "PicklesDoc.Pickles.MSBuild.Tasks.dll";
        msBuildDir + "build/Pickles.MSBuild.targets";]

    NuGet (fun p ->
        {p with
            OutputPath = deployDir
            WorkingDir = packagingDir
            Version = version
            Publish = false })
            "src/Pickles/Pickles.MSBuild/Pickles.MSBuild.nuspec"
)

Target "CreatePackagePowerShell" (fun _ ->
    // Copy all the package files into a package folder
    CopyFiles packagingDir [
        powerShellDir + "Autofac.dll";
        powerShellDir + "ClosedXML.dll";
        powerShellDir + "DocumentFormat.OpenXml.dll";
        powerShellDir + "Gherkin.dll";
        powerShellDir + "MarkdownDeep.dll";
        powerShellDir + "NDesk.Options.dll";
        powerShellDir + "Newtonsoft.Json.dll";
        powerShellDir + "NLog.dll";
        powerShellDir + "PicklesDoc.Pickles.Library.dll";
        powerShellDir + "PicklesDoc.Pickles.ObjectModel.dll";
        powerShellDir + "PicklesDoc.Pickles.TestFrameworks.dll";
        powerShellDir + "PicklesDoc.Pickles.PowerShell.dll";
        powerShellDir + "PicklesDoc.Pickles.DocumentationBuilders.Cucumber.dll";
        powerShellDir + "PicklesDoc.Pickles.DocumentationBuilders.Word.dll";
        powerShellDir + "PicklesDoc.Pickles.DocumentationBuilders.Excel.dll";
        powerShellDir + "PicklesDoc.Pickles.DocumentationBuilders.Json.dll";
        powerShellDir + "PicklesDoc.Pickles.DocumentationBuilders.Html.dll";
        powerShellDir + "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.dll";
        powerShellDir + "System.IO.Abstractions.dll";
        powerShellDir + "FeatureSwitcher.dll";
        powerShellDir + "Strike.Jint.dll";
        powerShellDir + "Jint.dll";
        "src/Pickles/Pickles.PowerShell/init.ps1"  ]

    NuGet (fun p ->
        {p with
            OutputPath = deployDir
            WorkingDir = packagingDir
            Version = version
            Publish = false })
            "src/Pickles/Pickles.PowerShell/Pickles.nuspec"
)

Target "CreatePackage" (fun _ ->
    trace ("Starting build of nuget packages version " + version)
    DeleteDir packagingDir
)

// Dependencies
"Clean"
  ==> "CreatePackageCommandLine"
  ==> "CreatePackageMsBuild"
  ==> "CreatePackagePowerShell"
  ==> "CreatePackage"


// start build
RunTargetOrDefault "CreatePackage"