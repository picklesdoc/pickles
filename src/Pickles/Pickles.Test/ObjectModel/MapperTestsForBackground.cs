//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapperTestsForBackground.cs" company="PicklesDoc">
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
    public class MapperTestsForBackground
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToScenarioBackground_NullBackground_ReturnsNullScenario()
        {
            var mapper = this.factory.CreateMapper();

            Scenario result = mapper.MapToScenario((G.Background)null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToScenarioBackground_RegularBackground_ReturnsScenario()
        {
            G.Background background = this.factory.CreateBackground(
                "Background",
                "Description of the Background",
                new[]
                {
                    this.factory.CreateStep("Given", "I enter '50' in the calculator"),
                });

            var mapper = this.factory.CreateMapper();

            Scenario result = mapper.MapToScenario(background);

            Check.That(result.Name).IsEqualTo("Background");
            Check.That(result.Description).IsEqualTo("Description of the Background");
            Check.That(result.Steps.Count).IsEqualTo(1);
            Check.That(result.Steps[0].Keyword).IsEqualTo(Keyword.Given);
            Check.That(result.Steps[0].Name).IsEqualTo("I enter '50' in the calculator");
            Check.That(result.Tags).IsEmpty();
        }

        [Test]
        public void MapToScenario_BackgroundWithNullDescription_ReturnsScenarioWithEmptyDescription()
        {
            G.Background background = this.factory.CreateBackground(
                "Background",
                null,
                new[]
                {
                    this.factory.CreateStep("Given", "unimportant step"),
                });

            var mapper = this.factory.CreateMapper();

            Scenario result = mapper.MapToScenario(background);

            Check.That(result.Description).IsEqualTo(string.Empty);
        }

        [Test]
        public void MapToFeature_Always_MapsFeaturePropertyOfBackground()
        {
            var scenario = this.factory.CreateScenario(
                new[] { "unimportant tag" },
                "My scenario title",
                null,
                new[] { this.factory.CreateStep("Given", "unimportant step") });
            var background = this.factory.CreateBackground(
                "Background",
                "Description of the Background",
                new[]
                {
                    this.factory.CreateStep("Given", "another unimportant step"),
                });
            var gherkinDocument = this.factory.CreateGherkinDocument(
                "My Feature",
                "My Description",
                scenarioDefinitions: new G.ScenarioDefinition[] { scenario },
                background: background);


            var mapper = this.factory.CreateMapper();

            var mappedFeature = mapper.MapToFeature(gherkinDocument);

            Check.That(mappedFeature.FeatureElements.Count).IsEqualTo(1);

            var mappedBackground = mappedFeature.Background;

            Check.That(mappedBackground.Feature).IsSameReferenceThan(mappedFeature);
        }
    }
}
