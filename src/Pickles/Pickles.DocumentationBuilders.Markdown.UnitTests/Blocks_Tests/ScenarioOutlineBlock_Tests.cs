//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioOutlineBlock_Tests.cs" company="PicklesDoc">
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
    public class ScenarioOutlineBlock_Tests
    {
        [Test]
        public void A_New_ScenarioOutlineBlock_Has_Scenario_Heading_On_First_Line()
        {
            var expectedString = "SOHF: Hello, World";
            var mockStyle = new MockStylist
            {
                ScenarioOutlineHeadingFormat = "SOHF: {0}"
            };
            var scenarioOutline = new ScenarioOutline
            {
                Name = "Hello, World"
            };

            var scenarioOutlineBlock = new ScenarioOutlineBlock(scenarioOutline,mockStyle);
            var actualString = scenarioOutlineBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual(expectedString, actualString[0]);
            Assert.AreEqual(2, actualString.Length);
        }

        [Test]
        public void When_ScenarioOutline_Tags_Available_They_Are_Placed_On_Single_Line_Before_Heading()
        {
            var mockStyle = new MockStylist
            {
                ScenarioOutlineHeadingFormat = "ScenarioOutlineHeading: {0}",
                TagFormat = ">>>{0}<<<"
            };
            var scenarioOutline = new ScenarioOutline
            {
                Name = "Scenario with Tags"
            };
            scenarioOutline.Tags.Add("tagone");
            scenarioOutline.Tags.Add("tagtwo");

            var scenarioOutlineBlock = new ScenarioOutlineBlock(scenarioOutline, mockStyle);
            var actualString = scenarioOutlineBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual(">>>tagone<<< >>>tagtwo<<<", actualString[0]);
            Assert.AreEqual("ScenarioOutlineHeading: Scenario with Tags", actualString[2]);
            Assert.AreEqual(4, actualString.Length);
        }

        [Test]
        public void When_A_ScenarioOutline_Step_Is_Available_It_Is_Placed_On_Single_Line_After_Heading()
        {
            var mockStyle = new MockStylist
            {
                ScenarioOutlineHeadingFormat = "ScenarioOutlineHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenarioOutline = new ScenarioOutline
            {
                Name = "ScenarioOutline with Step"
            };
            scenarioOutline.Steps.Add(new Step() { NativeKeyword = "Natkey ", Name = "I am a step" });

            var scenarioOutlineBlock = new ScenarioOutlineBlock(scenarioOutline, mockStyle);
            var actualString = scenarioOutlineBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("ScenarioOutlineHeading: ScenarioOutline with Step", actualString[0]);
            Assert.AreEqual("Keyword: Natkey Step: I am a step", actualString[2]);
            Assert.AreEqual(4, actualString.Length);
        }

        [Test]
        public void When_A_ScenarioOutline_Step_Is_Available_It_Has_Escape_Characters_For_Placeholder_Brackets()
        {
            var mockStyle = new MockStylist
            {
                ScenarioOutlineHeadingFormat = "ScenarioOutlineHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenarioOutline = new ScenarioOutline
            {
                Name = "ScenarioOutline with placeholder Step"
            };
            scenarioOutline.Steps.Add(new Step() { NativeKeyword = "Natkey ", Name = "I am a <placeholder> step" });

            var scenarioOutlineBlock = new ScenarioOutlineBlock(scenarioOutline, mockStyle);
            var actualString = scenarioOutlineBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("ScenarioOutlineHeading: ScenarioOutline with placeholder Step", actualString[0]);
            Assert.AreEqual(@"Keyword: Natkey Step: I am a \<placeholder\> step", actualString[2]);
            Assert.AreEqual(4, actualString.Length);
        }

        [Test]
        public void Examples_Are_Included_As_Table()
        {
            var mockStyle = new MockStylist
            {
                ScenarioOutlineHeadingFormat = "ScenarioOutlineHeading: {0}",
                ExampleHeadingFormat = "ExampleHeading: {0}",
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var scenarioOutline = new ScenarioOutline
            {
                Name = "ScenarioOutline with Examples"
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

            scenarioOutline.Examples = examples;

            var scenarioOutlineBlock = new ScenarioOutlineBlock(scenarioOutline, mockStyle);
            var actualString = scenarioOutlineBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("ScenarioOutlineHeading: ScenarioOutline with Examples", actualString[0]);
            Assert.AreEqual("ExampleHeading: My Examples", actualString[2]);
            Assert.AreEqual("> | example | val_one | val_two |", actualString[4]);
            Assert.AreEqual("> | --- | --- | --- |", actualString[5]);
            Assert.AreEqual("> | ex.one | one.one | one.two |", actualString[6]);
            Assert.AreEqual("> | ex.two | two.one | two.two |", actualString[7]);
            Assert.AreEqual(9, actualString.Length);
        }
    }
}
