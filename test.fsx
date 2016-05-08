// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake


// Properties
let testDir  = "./test/"

Target "Test" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.Test.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testDir + "TestResults.xml" })
)

Target "Test.TestFrameworks" (fun _ ->
    !! (testDir + "PicklesDoc.Pickles.TestFrameworks.UnitTests")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testDir + "TestFrameworks.TestResults.xml" })
)

"Test" ==> "Test.TestFrameworks"

// start build
RunTargetOrDefault "Test.TestFrameworks"