// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake

// Properties
let cmdDir = "./build/exe/"
let guiDir = "./build/gui/"
let deployDir  = "./deploy/chocolatey/"
let packagingDir = "./packaging/"
let chocoDir = "./chocolatey/"

// version info
let version = environVar "version" // or retrieve from CI server

Target "Clean" (fun _ ->
    CleanDirs [deployDir; packagingDir]
)


Target "CreatePackage CMD" (fun _ ->
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
        cmdDir + "NLog.config";
        "./LICENSE.txt";
        "./VERIFICATION.txt";]

    WriteFile (packagingDir + "version.ps1") [("$version = \"" + version + "\"")]
    NuGet (fun p ->
        {p with
            OutputPath = deployDir
            WorkingDir = packagingDir
            Version = version
            Publish = false })
            (chocoDir + "Pickles.nuspec")
)


Target "CreatePackage GUI" (fun _ ->
    CopyFiles packagingDir [
        guiDir + "Autofac.dll";
        guiDir + "ClosedXML.dll";
        guiDir + "DocumentFormat.OpenXml.dll";
        guiDir + "GalaSoft.MvvmLight.dll";
        guiDir + "GalaSoft.MvvmLight.Extras.dll";
        guiDir + "GalaSoft.MvvmLight.Platform.dll";
        guiDir + "Gherkin.dll";
        guiDir + "MahApps.Metro.dll";
        guiDir + "MarkdownDeep.dll";
        guiDir + "CommonServiceLocator.dll";
        guiDir + "ControlzEx.dll";
        guiDir + "Newtonsoft.Json.dll";
        guiDir + "NLog.dll";
        guiDir + "NlogViewer.dll";
        guiDir + "Ookii.Dialogs.Wpf.dll";
        guiDir + "PicklesDoc.Pickles.Library.dll";
        guiDir + "PicklesDoc.Pickles.ObjectModel.dll";
        guiDir + "PicklesDoc.Pickles.TestFrameworks.dll";
        guiDir + "PicklesDoc.Pickles.DocumentationBuilders.Cucumber.dll";
        guiDir + "PicklesDoc.Pickles.DocumentationBuilders.Word.dll";
        guiDir + "PicklesDoc.Pickles.DocumentationBuilders.Excel.dll";
        guiDir + "PicklesDoc.Pickles.DocumentationBuilders.Json.dll";
        guiDir + "PicklesDoc.Pickles.DocumentationBuilders.Html.dll";
        guiDir + "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.dll";
        guiDir + "System.IO.Abstractions.dll";
        guiDir + "FeatureSwitcher.dll";
        guiDir + "Strike.Jint.dll";
        guiDir + "Jint.dll";
        guiDir + "System.Windows.Interactivity.dll";
        guiDir + "picklesui.exe";
        guiDir + "NLog.config";
        guiDir + "PicklesUI.exe.config";
        "./LICENSE.txt";
        "./VERIFICATION.txt";]

    WriteFile (packagingDir + "version.ps1") [("$version = \"" + version + "\"")]
    WriteFile (packagingDir + "picklesui.exe.gui") [("")]
    NuGet (fun p ->
        {p with
            OutputPath = deployDir
            WorkingDir = packagingDir
            Version = version
            Publish = false })
            (chocoDir + "picklesui.nuspec")
)


Target "Default" (fun _ ->
    trace ("Starting build of Pickles version " + version)
    DeleteDir packagingDir
)


// Dependencies
"Clean"
  ==> "CreatePackage CMD"
  ==> "CreatePackage GUI"
  ==> "Default"


// start build
RunTargetOrDefault "Default"

