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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class FeatureBlock_Tests
    {
        [Test]
        public void A_New_FeatureBlock_Has_Title_From_Feature()
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
            string actualString = featureBlock.ToString();

            Assert.IsTrue(
                actualString.Contains(expectedString),
                string.Format("String \"{0}\" not found in \"{1}\"", expectedString, actualString)
                );
        }
    }
}
