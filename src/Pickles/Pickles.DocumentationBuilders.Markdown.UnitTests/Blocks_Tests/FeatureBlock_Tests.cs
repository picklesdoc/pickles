//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureBlock_Tests.cs" company="PicklesDoc">
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
    public class FeatureBlock_Tests
    {
        [Test]
        public void A_New_FeatureBlock_Has_Feature_Heading_On_First_Line()
        {
            var expectedString = "FHF: Hello, World";
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FHF: {0}"
            };
            var feature = new Feature
            {
                Name = "Hello, World"
            };

            var featureBlock = new FeatureBlock(feature,mockStyle);
            var actualString = featureBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual(expectedString, actualString[0]);
            Assert.AreEqual(3, actualString.Length);
        }

        [Test]
        public void When_Feature_Description_Available_It_Is_Placed_Below_Feature_Heading()
        {
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FeatureHeading: {0}"
            };
            var feature = new Feature
            {
                Name = "Feature with description",
                Description = String.Concat(
                    "In order to determine that world is flat", Environment.NewLine,
                    "As a captain of a ship", Environment.NewLine,
                    "I want to sail beyond the horizion")
            };

            var featureBlock = new FeatureBlock(feature, mockStyle);
            var actualString = featureBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("FeatureHeading: Feature with description", actualString[0]);
            Assert.AreEqual("In order to determine that world is flat", actualString[2]);
            Assert.AreEqual("As a captain of a ship", actualString[4]);
            Assert.AreEqual("I want to sail beyond the horizion", actualString[6]);
            Assert.AreEqual(9, actualString.Length);
        }

        [Test]
        public void When_Feature_Tags_Available_They_Are_Placed_On_Single_Line_Before_Heading()
        {
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FeatureHeading: {0}",
                TagFormat = ">>>{0}<<<"
            };
            var feature = new Feature
            {
                Name = "Feature with Tags"
            };
            feature.AddTag("tagone");
            feature.AddTag("tagtwo");

            var featureBlock = new FeatureBlock(feature, mockStyle);
            var actualString = featureBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual(">>>tagone<<< >>>tagtwo<<<", actualString[0]);
            Assert.AreEqual("FeatureHeading: Feature with Tags", actualString[2]);
            Assert.AreEqual(5, actualString.Length);
        }

        [Test]
        public void When_Scenario_Is_Available_It_Is_Included_After_Heading()
        {
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FeatureHeading: {0}",
                ScenarioHeadingFormat = "ScenarioHeading: {0}"
            };
            var feature = new Feature
            {
                Name = "Feature with Scenario"
            };
            feature.AddFeatureElement(new Scenario() { Name = "My Scenario" });

            var featureBlock = new FeatureBlock(feature, mockStyle);
            var actualString = featureBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("FeatureHeading: Feature with Scenario", actualString[0]);
            Assert.AreEqual("ScenarioHeading: My Scenario", actualString[2]);
            Assert.AreEqual(5, actualString.Length);
        }

        [Test]
        public void When_ScenarioOutline_Is_Available_It_Is_Included_After_Heading()
        {
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FeatureHeading: {0}",
                ScenarioOutlineHeadingFormat = "ScenarioOutlineHeading: {0}"
            };
            var feature = new Feature
            {
                Name = "Feature with Scenario Outline"
            };
            feature.AddFeatureElement(new ScenarioOutline() { Name = "My Scenario Outline" });

            var featureBlock = new FeatureBlock(feature, mockStyle);
            var actualString = featureBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("FeatureHeading: Feature with Scenario Outline", actualString[0]);
            Assert.AreEqual("ScenarioOutlineHeading: My Scenario Outline", actualString[2]);
            Assert.AreEqual(5, actualString.Length);
        }

        [Test]
        public void When_Mutiple_Scenarios_Are_Available_They_Are_Included_After_Heading()
        {
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FeatureHeading: {0}",
                ScenarioHeadingFormat = "ScenarioHeading: {0}"
            };
            var feature = new Feature
            {
                Name = "Feature with Scenario"
            };
            feature.AddFeatureElement(new Scenario() { Name = "My Scenario one" });
            feature.AddFeatureElement(new Scenario() { Name = "My Scenario two" });

            var featureBlock = new FeatureBlock(feature, mockStyle);
            var actualString = featureBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("FeatureHeading: Feature with Scenario", actualString[0]);
            Assert.AreEqual("ScenarioHeading: My Scenario one", actualString[2]);
            Assert.AreEqual("ScenarioHeading: My Scenario two", actualString[4]);
            Assert.AreEqual(7, actualString.Length);
        }

        [Test]
        public void When_A_Background_Is_Present_Its_Included_In_Markdown_Output()
        {
            var mockStyle = new MockStylist
            {
                FeatureHeadingFormat = "FeatureHeading: {0}",
                BackgroundHeadingFormat = "BackgroundHeading: {0}"
            };
            var feature = new Feature
            {
                Name = "Feature with Background"
            };
            feature.AddBackground(new Scenario());

            var featureBlock = new FeatureBlock(feature, mockStyle);
            var results = new string[featureBlock.Lines.Count];
            var i = 0;
            foreach (var line in featureBlock.Lines)
            {
                results[i] = line;
                i++;
            }

            Assert.AreEqual("FeatureHeading: Feature with Background", results[0]);
            Assert.AreEqual("BackgroundHeading:", results[2]);
            Assert.AreEqual(3, results.Length);
        }
    }
}
