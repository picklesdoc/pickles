//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapperTestsForFeature.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.ObjectModel;
using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.Test.ObjectModel
{
    [TestFixture]
    public class MapperTestsForFeature
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToFeature_NullFeature_ReturnsNull()
        {
            var mapper = this.factory.CreateMapper();

            Feature result = mapper.MapToFeature(null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToFeature_FeatureWithSimpleProperties_ReturnsFeaturesWithThoseProperties()
        {
            G.Feature feature = this.factory.CreateFeature("Title of the feature", "Description of the feature");

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(feature);

            Check.That(result.Name).IsEqualTo("Title of the feature");
            Check.That(result.Description).IsEqualTo("Description of the feature");
        }

        [Test]
        public void MapToFeature_FeatureWithScenarioDefinitions_ReturnsFeatureWithFeatureElements()
        {
            var feature = this.factory.CreateFeature(
                "My Feature",
                string.Empty,
                scenarioDefinitions: new G.ScenarioDefinition[]
                {
                    this.factory.CreateScenario(new string[0], "My scenario", string.Empty, new G.Step[0]),
                    this.factory.CreateScenarioOutline(new string[0], "My scenario outline", string.Empty, new G.Step[0], new G.Examples[0])
                });

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(feature);

            Check.That(result.FeatureElements.Count).IsEqualTo(2);
            Check.That(result.FeatureElements[0].Name).IsEqualTo("My scenario");
            Check.That(result.FeatureElements[1].Name).IsEqualTo("My scenario outline");
        }

        [Test]
        public void MapToFeature_FeatureWithBackground_ReturnsFeatureWithBackground()
        {
            var feature = this.factory.CreateFeature(
                "My Feature",
                string.Empty,
                background: this.factory.CreateBackground("My background", "My description", new G.Step[0]));

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(feature);

            Check.That(result.Background.Name).IsEqualTo("My background");
            Check.That(result.Background.Description).IsEqualTo("My description");
        }

        [Test]
        public void MapToFeature_FeatureWithTags_ReturnsFeatureWithTags()
        {
            var feature = this.factory.CreateFeature("My Feature", string.Empty, tags: new[] { "my tag 1", "my tag 2" });

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(feature);

            Check.That(result.Tags).ContainsExactly("my tag 1", "my tag 2");
        }

        [Test]
        public void MapToFeature_FeatureWithNullDescription_ReturnsFeatureWithEmptyDescription()
        {
            var feature = this.factory.CreateFeature("My Feature", null);

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(feature);

            Check.That(result.Description).Equals(string.Empty);
        }
    }
}
