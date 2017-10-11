//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapperTestsForScenarioOutline.cs" company="PicklesDoc">
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
    public class MapperTestsForScenarioOutline
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToScenario_NullScenario_ReturnsNull()
        {
            var mapper = this.factory.CreateMapper();

            ScenarioOutline result = mapper.MapToScenarioOutline(null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToScenario_RegularScenario_ReturnsScenario()
        {
            var scenarioOutline = this.factory.CreateScenarioOutline(
                new[] { "myTag1", "myTag2" },
                "My scenario title",
                "Description of the scenario",
                new[]
                {
                    this.factory.CreateStep("Given", "I enter '50' in the calculator"),
                    this.factory.CreateStep("When", "I press 'plus' on the calculator"),
                    this.factory.CreateStep("Then", "the screen shows '50'")
                },
                new[]
                {
                    this.factory.CreateExamples(
                        "Examples",
                        "My Description",
                        new[] { "Header 1", "Header 2" },
                        new[]
                        {
                            new[] { "Row 1, Value 1", "Row 2, Value 2" },
                            new[] { "Row 2, Value 1", "Row 2, Value 2" }
                        })
                });

            var mapper = this.factory.CreateMapper();

            ScenarioOutline result = mapper.MapToScenarioOutline(scenarioOutline);

            Check.That(result.Name).IsEqualTo("My scenario title");

            Check.That(result.Description).IsEqualTo("Description of the scenario");

            Check.That(result.Steps.Count).IsEqualTo(3);
            Check.That(result.Steps[0].Keyword).IsEqualTo(Keyword.Given);
            Check.That(result.Steps[0].Name).IsEqualTo("I enter '50' in the calculator");
            Check.That(result.Steps[1].Keyword).IsEqualTo(Keyword.When);
            Check.That(result.Steps[1].Name).IsEqualTo("I press 'plus' on the calculator");
            Check.That(result.Steps[2].Keyword).IsEqualTo(Keyword.Then);
            Check.That(result.Steps[2].Name).IsEqualTo("the screen shows '50'");

            Check.That(result.Tags.Count).IsEqualTo(2);
            Check.That(result.Tags[0]).IsEqualTo("myTag1");
            Check.That(result.Tags[1]).IsEqualTo("myTag2");

            Check.That(result.Examples.Count).IsEqualTo(1);
            Check.That(result.Examples[0].Name).IsEqualTo("Examples");
            Check.That(result.Description).IsEqualTo("Description of the scenario");
            Check.That(result.Examples[0].TableArgument.HeaderRow.Cells).ContainsExactly("Header 1", "Header 2");
            Check.That(result.Examples[0].TableArgument.DataRows.Count).IsEqualTo(2);
            Check.That(result.Examples[0].TableArgument.DataRows[0].Cells).ContainsExactly("Row 1, Value 1", "Row 2, Value 2");
            Check.That(result.Examples[0].TableArgument.DataRows[1].Cells).ContainsExactly("Row 2, Value 1", "Row 2, Value 2");
        }

        [Test]
        public void MapToScenarioOutline_TitleSpecialCharacters_HasValidSlug()
        {
            var scenarioOutline = this.factory.CreateScenarioOutline(
                new string[0],
                "My $super%-crypTIC @(SCENARIO)+= *Title#^!   It's Got \\| Some W1ld?/ Ch<>arac~`ters + Cyrilic Жят ед ыррор",
                string.Empty,
                new G.Step[0], 
                new G.Examples[0]);

            var mapper = this.factory.CreateMapper();

            ScenarioOutline result = mapper.MapToScenarioOutline(scenarioOutline);

            Check.That(result.Slug).IsEqualTo("my-super-cryptic-scenario-title-its-got-some-w1ld-characters-cyrilic");
        }

        [Test]
        public void MapToScenarioOutline_ScenarioOutlineWithNullDescription_ReturnsScenarioOutlineWithEmptyDescription()
        {
            var scenarioOutline = this.factory.CreateScenarioOutline(
                new[] { "unimportant tag" },
                "My scenario outline title",
                null,
                new[] { this.factory.CreateStep("Given", "unimportant step") },
                null);

            var mapper = this.factory.CreateMapper();

            ScenarioOutline result = mapper.MapToScenarioOutline(scenarioOutline);

            Check.That(result.Description).IsEqualTo(string.Empty);
        }

        [Test]
        public void MapToScenarioOutline_Always_MapsFeatureProperty()
        {
            var scenarioOutline = this.factory.CreateScenarioOutline(
                new[] { "unimportant tag" },
                "My scenario outline title",
                null,
                new[] { this.factory.CreateStep("Given", "unimportant step") },
                null);
            var gherkinDocument = this.factory.CreateGherkinDocument(
                "My Feature",
                "My Description",
                scenarioDefinitions: new G.ScenarioDefinition[] { scenarioOutline });


            var mapper = this.factory.CreateMapper();

            var mappedFeature = mapper.MapToFeature(gherkinDocument);

            Check.That(mappedFeature.FeatureElements.Count).IsEqualTo(1);

            var mappedScenarioOutline = mappedFeature.FeatureElements[0] as ScenarioOutline;

            Check.That(mappedScenarioOutline.Feature).IsSameReferenceAs(mappedFeature);
        }
    }
}
