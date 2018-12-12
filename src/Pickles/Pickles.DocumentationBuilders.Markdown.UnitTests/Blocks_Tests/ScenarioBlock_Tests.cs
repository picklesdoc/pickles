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

            Assert.AreEqual(expectedString, actualString[0]);
            Assert.AreEqual(2, actualString.Length);
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

            Assert.AreEqual(">>>tagone<<< >>>tagtwo<<<", actualString[0]);
            Assert.AreEqual("ScenarioHeading: Scenario with Tags", actualString[2]);
            Assert.AreEqual(4, actualString.Length);
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

            Assert.AreEqual("ScenarioHeading: Scenario with Step", actualString[0]);
            Assert.AreEqual("Keyword: Natkey Step: I am a step", actualString[2]);
            Assert.AreEqual(4, actualString.Length);
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

            Assert.AreEqual("ScenarioHeading: result Scenario with Step", actualString[0]);
            Assert.AreEqual(2, actualString.Length);
        }
    }
}
