//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioBlock_Tests.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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
using PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks;
using PicklesDoc.Pickles.ObjectModel;
using System;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class ScenarioBlock_Tests
    {
        [Test]
        public void A_New_ScenarioBlock_Has_Scenario_Heading_On_First_Line()
        {
            var expectedString = "SHF: Hello, World";
            var mockStyle = new MockStylist
            {
                ScenarioHeadingFormat = "SHF: {0}"
            };
            var scenario = new Scenario
            {
                Name = "Hello, World"
            };

            var scenarioBlock = new ScenarioBlock(scenario,mockStyle);
            var actualString = scenarioBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That(expectedString, Is.EqualTo(actualString[0]));
            Assert.That(2, Is.EqualTo(actualString.Length));
        }

        [Test]
        public void When_Scenario_Tags_Available_They_Are_Placed_On_Single_Line_Before_Heading()
        {
            var mockStyle = new MockStylist
            {
                ScenarioHeadingFormat = "ScenarioHeading: {0}",
                TagFormat = ">>>{0}<<<"
            };
            var scenario = new Scenario
            {
                Name = "Scenario with Tags"
            };
            scenario.Tags.Add("tagone");
            scenario.Tags.Add("tagtwo");

            var scenarioBlock = new ScenarioBlock(scenario, mockStyle);
            var actualString = scenarioBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That(">>>tagone<<< >>>tagtwo<<<", Is.EqualTo(actualString[0]));
            Assert.That("ScenarioHeading: Scenario with Tags", Is.EqualTo(actualString[2]));
            Assert.That(4, Is.EqualTo(actualString.Length));
        }

        [Test]
        public void When_A_Scenario_Step_Is_Available_It_Is_Placed_On_Single_Line_After_Heading()
        {
            var mockStyle = new MockStylist
            {
                ScenarioHeadingFormat = "ScenarioHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenario = new Scenario
            {
                Name = "Scenario with Step"
            };
            scenario.Steps.Add(new Step() { NativeKeyword = "Natkey ", Name = "I am a step" });

            var scenarioBlock = new ScenarioBlock(scenario, mockStyle);
            var actualString = scenarioBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("ScenarioHeading: Scenario with Step", Is.EqualTo(actualString[0]));
            Assert.That("Keyword: Natkey Step: I am a step", Is.EqualTo(actualString[2]));
            Assert.That(4, Is.EqualTo(actualString.Length));
        }

        [Test]
        public void When_A_Scenario_Step_Has_A_Result__The_Result_Is_Included()
        {
            var mockStyle = new MockStylist
            {
                ScenarioHeadingFormat = "ScenarioHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenario = new Scenario
            {
                Name = "Scenario with Step",
                Result = TestResult.Passed
            };

            var scenarioBlock = new ScenarioBlock(scenario, mockStyle);
            var actualString = scenarioBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("ScenarioHeading: result Scenario with Step", Is.EqualTo(actualString[0]));
            Assert.That(2, Is.EqualTo(actualString.Length));
        }
		
        [Test]
        public void When_A_Scenario_Step_Is_Available_It_Has_Escape_Characters_For_Placeholder_Brackets()
        {
            var mockStyle = new MockStylist
            {
                ScenarioHeadingFormat = "ScenarioHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenario = new Scenario
            {
                Name = "Scenario with placeholder Step"
            };
            scenario.Steps.Add(new Step() { NativeKeyword = "Natkey ", Name = "I am a <placeholder> step" });

            var scenarioBlock = new ScenarioBlock(scenario, mockStyle);
            var actualString = scenarioBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("ScenarioHeading: Scenario with placeholder Step", Is.EqualTo(actualString[0]));
            Assert.That(@"Keyword: Natkey Step: I am a \<placeholder\> step", Is.EqualTo(actualString[2]));
            Assert.That(4, Is.EqualTo(actualString.Length));
        }

        [Test]
        public void Examples_Are_Included_As_Table()
        {
            var mockStyle = new MockStylist
            {
                ScenarioHeadingFormat = "ScenarioHeading: {0}",
                ExampleHeadingFormat = "ExampleHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenario = new Scenario
            {
                Name = "Scenario with Examples"
            };

            var examplesTable = new ObjectModel.ExampleTable
            {
                HeaderRow = new ObjectModel.TableRow(new string[] { "example","val_one", "val_two"}),

                DataRows = new System.Collections.Generic.List<ObjectModel.TableRow>()
            };

            examplesTable.DataRows.Add(new ObjectModel.TableRow(new string[] { "ex.one", "one.one", "one.two" }));
            examplesTable.DataRows.Add(new ObjectModel.TableRow(new string[] { "ex.two", "two.one", "two.two" }));


            var example = new Example()
            {
                Name = "My Examples",
                TableArgument = examplesTable
            };

            var examples = new System.Collections.Generic.List<Example>() { example };

            scenario.Examples = examples;

            var scenarioBlock = new ScenarioBlock(scenario, mockStyle);
            var actualString = scenarioBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("ScenarioHeading: Scenario with Examples", Is.EqualTo(actualString[0]));
            Assert.That("ExampleHeading: My Examples", Is.EqualTo(actualString[2]));
            Assert.That("> | example | val_one | val_two |", Is.EqualTo(actualString[4]));
            Assert.That("> | --- | --- | --- |", Is.EqualTo(actualString[5]));
            Assert.That("> | ex.one | one.one | one.two |", Is.EqualTo(actualString[6]));
            Assert.That("> | ex.two | two.one | two.two |", Is.EqualTo(actualString[7]));
            Assert.That(9, Is.EqualTo(actualString.Length));
        }
    }
}
