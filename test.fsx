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

"Test" ==> "Test.TestFrameworks"

// start build
RunTargetOrDefault "Test.TestFrameworks"