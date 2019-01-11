//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IJsonFeatureElementExtensions.cs" company="PicklesDoc">
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

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json.UnitTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class IJsonFeatureElementExtensionsTests
    {
        [Test]
        public void AllTags_NullArgument_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => 
                ((IJsonFeatureElement)null).AllTags());
        }

        [Test]
        public void AllTags_NoTagsInFeatureElementAndInFeature_ReturnsEmptyList()
        {
            var scenario = new JsonScenario { Feature = new JsonFeature() };

            IReadOnlyList<string> tags = scenario.AllTags();

            Assert.IsEmpty(tags);
        }

        [Test]
        public void AllTags_TagsInFeatureElement_ReturnsTags()
        {
            var scenario = new JsonScenario
            {
                Feature = new JsonFeature(),
                Tags = new List<string>
                {
                    "tag1",
                    "tag2"
                }
            };

            IReadOnlyList<string> tags = scenario.AllTags();

            CollectionAssert.AreEquivalent(new[] { "tag1", "tag2" }, tags);
        }

        [Test]
        public void AllTags_TagsInScenarioElement_ReturnsTags()
        {
            var scenario = new JsonScenario
            {
                Feature = new JsonFeature
                {
                    Tags = new List<string>
                    {
                        "tag1",
                        "tag2"
                    }
                }
            };

            IReadOnlyList<string> tags = scenario.AllTags();

            CollectionAssert.AreEquivalent(new[] { "tag1", "tag2" }, tags);
        }

        [Test]
        public void AllTags_TagsBothInScenarioAndInFeatureElement_ReturnsEachTagOnlyOnce()
        {
            var scenario = new JsonScenario
            {
                Feature = new JsonFeature
                {
                    Tags = new List<string>
                    {
                        "tag1"
                    }
                },
                Tags = new List<string>
                {
                    "tag1"
                }
            };

            IReadOnlyList<string> tags = scenario.AllTags();

            CollectionAssert.AreEquivalent(new[] { "tag1" }, tags);
        }
    }
}