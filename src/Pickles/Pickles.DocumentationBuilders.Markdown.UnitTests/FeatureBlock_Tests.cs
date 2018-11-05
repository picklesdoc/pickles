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
            Assert.AreEqual("In order to determine that world is flat", actualString[1]);
            Assert.AreEqual("As a captain of a ship", actualString[2]);
            Assert.AreEqual("I want to sail beyond the horizion", actualString[3]);
        }
    }
}
