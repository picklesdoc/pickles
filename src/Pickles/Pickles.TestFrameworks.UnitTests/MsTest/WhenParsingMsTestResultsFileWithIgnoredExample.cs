//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingMsTestResultsFileWithEmptyExampleValues.cs" company="PicklesDoc">
//  Copyright 2011 Jeffrey Cameron
//  Copyright 2012-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------


using NUnit.Framework;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.TestFrameworks.MsTest;
using System.Collections.Generic;
using NFluent;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.MsTest
{
    [TestFixture]
    public class WhenParsingMsTestResultsFileWithIgnoredExample : StandardTestSuite<MsTestResults>
    {
        public WhenParsingMsTestResultsFileWithIgnoredExample()
            : base("MsTest." + "results-example-mstest-ignoredexample.trx")
        {
        }

        [Test]
        public void ThenIgnoredScenarioOutlineIsSetToInconclusive()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Example With Ignored Scenario Outline" };
            var scenarioOutline = new ScenarioOutline { Name = "Add two numbers", Feature = feature };
            scenarioOutline.Steps = new List<Step>();

            var examples = new ExampleTable();
            examples.HeaderRow = new TableRow();
            examples.HeaderRow.Cells.Add("TestCase");
            var row = new TableRowWithTestResult();
            row.Cells.Add("1");
            examples.DataRows = new List<TableRow>();
            examples.DataRows.Add(row);

            scenarioOutline.Examples = new List<Example>();
            scenarioOutline.Examples.Add(new Example() { TableArgument = examples });

            var matchedExampleResult = results.GetExampleResult(scenarioOutline, new string[] { "1" });
            Check.That(matchedExampleResult).IsEqualTo(TestResult.Passed);

            var nonMatchExampleResult = results.GetExampleResult(scenarioOutline, new string[] { "2" });
            Check.That(nonMatchExampleResult).IsEqualTo(TestResult.Inconclusive);
        }
    }
}
