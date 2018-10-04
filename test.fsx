// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake
open Fake.Testing


// Properties
let testDir  = "./test/"

Target "Test" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.Test.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.Test.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.TestFrameworks" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.TestFrameworks.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.TestFrameworks.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.DocumentationBuilders.Cucumber" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.DocumentationBuilders.Cucumber.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.DocumentationBuilders.Cucumber.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.DocumentationBuilders.Dhtml" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.DocumentationBuilders.Excel" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.DocumentationBuilders.Excel.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.DocumentationBuilders.Excel.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.DocumentationBuilders.Html" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.DocumentationBuilders.Json" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.DocumentationBuilders.Json.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.DocumentationBuilders.Json.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.DocumentationBuilders.Word" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.DocumentationBuilders.Word.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.DocumentationBuilders.Word.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)


Target "Test.Runners.CommandLine" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.CommandLine.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.CommandLine.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

Target "Test.Runners.UI" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.UserInterface.UnitTests.dll")
      |> NUnit3 (fun p ->
          {p with
             ShadowCopy = false;
             OutputDir = testDir + "PicklesDoc.Pickles.UserInterface.UnitTests.TestResults.xml";
             ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" })
)

"Test"
    ==> "Test.DocumentationBuilders.Cucumber"
    ==> "Test.DocumentationBuilders.Dhtml"
    ==> "Test.DocumentationBuilders.Excel"
    ==> "Test.DocumentationBuilders.Html"
    ==> "Test.DocumentationBuilders.Json"
    ==> "Test.DocumentationBuilders.Word"
    ==> "Test.Runners.CommandLine"
    ==> "Test.Runners.UI"
    ==> "Test.TestFrameworks"

// start build
RunTargetOrDefault "Test.TestFrameworks"