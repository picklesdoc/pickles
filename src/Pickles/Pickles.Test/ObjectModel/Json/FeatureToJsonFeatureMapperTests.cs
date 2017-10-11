//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureToJsonFeatureMapperTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class FeatureToJsonFeatureMapperTests
    {
        [Test]
        public void Map_NullFeature_ReturnsNullJsonFeature()
        {
            var mapper = CreateMapper();

            JsonFeature actual = mapper.Map(null);

            Check.That(actual).IsNull();
        }

        private static FeatureToJsonFeatureMapper CreateMapper()
        {
            return new FeatureToJsonFeatureMapper(new LanguageServicesRegistry());
        }

        [Test]
        public void Map_SomeFeature_ReturnsJsonFeature()
        {
            var feature = new Feature();

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual).IsNotNull();
        }

        [Test]
        public void Map_FeatureWithName_ReturnsName()
        {
            var feature = new Feature { Name = "Name of the Feature" };

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.Name).IsEqualTo("Name of the Feature");
        }

        [Test]
        public void Map_FeatureWithDescription_ReturnsDescription()
        {
            var feature = new Feature { Description = "As a math idiot" };

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.Description).IsEqualTo("As a math idiot");
        }

        [Test]
        public void Map_FeatureWithElements_ReturnsListofElements()
        {
            var feature = new Feature
            {
                FeatureElements =
                {
                    new Scenario { Name = "Name of the Scenario" }
                }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.FeatureElements[0].Name).IsEqualTo("Name of the Scenario");
            Check.That(actual.FeatureElements[0].Feature).IsSameReferenceAs(actual);
        }

        [Test]
        public void Map_FeatureWithScenarioOutline_ReturnsScenarioOutline()
        {
            var feature = new Feature
            {
                FeatureElements =
                {
                    new ScenarioOutline {Name = "Name of the Scenario Outline"},
                }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.FeatureElements[0]).IsInstanceOf<JsonScenarioOutline>();
            Check.That(actual.FeatureElements[0].Name).IsEqualTo("Name of the Scenario Outline");
            Check.That(actual.FeatureElements[0].Feature).IsSameReferenceAs(actual);
        }

        [Test]
        public void Map_FeatureWithBogusFeatureElement_ShouldThrowArgumentException()
        {
            var feature = new Feature
            {
                FeatureElements =
                {
                    new BogusFeatureElement(),
                }
            };

            var mapper = CreateMapper();

            Check.ThatCode(() => mapper.Map(feature)).Throws<ArgumentException>();
        }

        private class BogusFeatureElement : IFeatureElement
        {
            public string Description { get; set; }
            public Feature Feature { get; set; }
            public string Name { get; set; }
            public string Slug { get; }
            public List<Step> Steps { get; set; }
            public List<string> Tags { get; set; }
            public TestResult Result { get; set; }
            public Location Location { get; set; }
        }

        [Test]
        public void Map_WithoutBackground_ReturnsNullBackground()
        {
            var feature = new Feature();

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.Background).IsNull();
        }

        [Test]
        public void Map_WithBackground_ReturnsBackground()
        {
            var feature = new Feature();
            feature.AddBackground(
                new Scenario
                {
                    Description = "The description of the background"
                });

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.Background.Description).IsEqualTo("The description of the background");
            Check.That(actual.Background.Feature).IsSameReferenceAs(actual);
        }

        [Test]
        public void Map_WithResult_ReturnsJsonTestResult()
        {
            var feature = new Feature { Result = TestResult.Passed };

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.Result.WasExecuted).IsTrue();
            Check.That(actual.Result.WasSuccessful).IsTrue();
        }

        [Test]
        public void Map_WithTags_ReturnsTags()
        {
            var feature = new Feature { Tags = { "tag" } };

            var mapper = CreateMapper();

            var actual = mapper.Map(feature);

            Check.That(actual.Tags).ContainsExactly("tag");
        }
    }
}