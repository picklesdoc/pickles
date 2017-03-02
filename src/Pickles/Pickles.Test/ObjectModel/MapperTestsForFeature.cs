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
using System.Linq;
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
            var gherkinDocument = this.factory.CreateGherkinDocument("Title of the feature", "Description of the feature");

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.Name).IsEqualTo("Title of the feature");
            Check.That(result.Description).IsEqualTo("Description of the feature");
        }

        [Test]
        public void MapToFeature_FeatureWithScenarioDefinitions_ReturnsFeatureWithFeatureElements()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument(
                "My Feature",
                string.Empty,
                scenarioDefinitions: new G.ScenarioDefinition[]
                {
                    this.factory.CreateScenario(new string[0], "My scenario", string.Empty, new G.Step[0]),
                    this.factory.CreateScenarioOutline(new string[0], "My scenario outline", string.Empty, new G.Step[0], new G.Examples[0])
                });

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.FeatureElements.Count).IsEqualTo(2);
            Check.That(result.FeatureElements[0].Name).IsEqualTo("My scenario");
            Check.That(result.FeatureElements[1].Name).IsEqualTo("My scenario outline");
        }

        [Test]
        public void MapToFeature_FeatureWithBackground_ReturnsFeatureWithBackground()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument(
                "My Feature",
                string.Empty,
                background: this.factory.CreateBackground("My background", "My description", new G.Step[0]));

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.Background.Name).IsEqualTo("My background");
            Check.That(result.Background.Description).IsEqualTo("My description");
        }

        [Test]
        public void MapToFeature_FeatureWithTags_ReturnsFeatureWithTags()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument("My Feature", string.Empty, tags: new[] { "my tag 1", "my tag 2" });

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.Tags).ContainsExactly("my tag 1", "my tag 2");
        }

        [Test]
        public void MapToFeature_FeatureWithNullDescription_ReturnsFeatureWithEmptyDescription()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument("My Feature", null);

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.Description).Equals(string.Empty);
        }

        [Test]
        public void MapToFeature_FeatureWithComments_ReturnsFeatureWithComments()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument(
                "My Feature",
                string.Empty,
                location: new G.Location(2, 0),
                comments: new G.Comment[]
                {
                    this.factory.CreateComment("# single line comment before the given step", 4, 4),
                    this.factory.CreateComment("# multiline comment before the then step - line 1", 6, 4),
                    this.factory.CreateComment("# multiline comment before the then step - line 2", 7, 4),
                    this.factory.CreateComment("# line comment before the last step", 10, 4),
                    this.factory.CreateComment("# line comment after the last step", 12, 4),
                },
                scenarioDefinitions: new G.ScenarioDefinition[]
                {
                    this.factory.CreateScenario(
                        new string[0], "My scenario", string.Empty,
                        new G.Step[]
                        {
                            this.factory.CreateStep("Given", "I am on the first step", 5, 4),
                            this.factory.CreateStep("When", "I am on the second step", 8, 4),
                            this.factory.CreateStep("When", "there is a third step without comment", 9, 4),
                            this.factory.CreateStep("Then", "I am on the last step", 11, 4)
                        },
                        location: new G.Location(3, 0)
                    )
                });

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);
            var scenario = result.FeatureElements[0];

            Check.That(scenario.Steps[0].Comments.Count).IsEqualTo(1);
            Check.That(scenario.Steps[0].Comments.Count(o => o.Type == CommentType.StepComment)).IsEqualTo(1);
            Check.That(scenario.Steps[0].Comments.Count(o => o.Type == CommentType.AfterLastStepComment)).IsEqualTo(0);
            Check.That(scenario.Steps[0].Comments[0].Text).IsEqualTo("# single line comment before the given step");

            Check.That(scenario.Steps[1].Comments.Count).IsEqualTo(2);
            Check.That(scenario.Steps[1].Comments.Count(o => o.Type == CommentType.StepComment)).IsEqualTo(2);
            Check.That(scenario.Steps[1].Comments.Count(o => o.Type == CommentType.AfterLastStepComment)).IsEqualTo(0);
            Check.That(scenario.Steps[1].Comments[0].Text).IsEqualTo("# multiline comment before the then step - line 1");
            Check.That(scenario.Steps[1].Comments[1].Text).IsEqualTo("# multiline comment before the then step - line 2");

            Check.That(scenario.Steps[2].Comments.Count).IsEqualTo(0);
            Check.That(scenario.Steps[2].Comments.Count(o => o.Type == CommentType.StepComment)).IsEqualTo(0);
            Check.That(scenario.Steps[2].Comments.Count(o => o.Type == CommentType.AfterLastStepComment)).IsEqualTo(0);

            Check.That(scenario.Steps[3].Comments.Count).IsEqualTo(2);
            Check.That(scenario.Steps[3].Comments.Count(o => o.Type == CommentType.StepComment)).IsEqualTo(1);
            Check.That(scenario.Steps[3].Comments.Count(o => o.Type == CommentType.AfterLastStepComment)).IsEqualTo(1);
            Check.That(scenario.Steps[3].Comments[0].Text).IsEqualTo("# line comment before the last step");
            Check.That(scenario.Steps[3].Comments[1].Text).IsEqualTo("# line comment after the last step");
        }

        [Test]
        public void MapToFeature_FeatureWithComments_ReturnsFeatureWithoutComments_WhenConfigurationSpecifies()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument(
                "My Feature",
                string.Empty,
                location: new G.Location(2, 0),
                comments: new G.Comment[]
                {
                    this.factory.CreateComment("# single line comment before the given step", 4, 4),
                    this.factory.CreateComment("# multiline comment before the then step - line 1", 6, 4),
                    this.factory.CreateComment("# multiline comment before the then step - line 2", 7, 4),
                    this.factory.CreateComment("# line comment before the last step", 10, 4),
                    this.factory.CreateComment("# line comment after the last step", 12, 4),
                },
                scenarioDefinitions: new G.ScenarioDefinition[]
                {
                    this.factory.CreateScenario(
                        new string[0], "My scenario", string.Empty,
                        new G.Step[]
                        {
                            this.factory.CreateStep("Given", "I am on the first step", 5, 4),
                            this.factory.CreateStep("When", "I am on the second step", 8, 4),
                            this.factory.CreateStep("When", "there is a third step without comment", 9, 4),
                            this.factory.CreateStep("Then", "I am on the last step", 11, 4)
                        },
                        location: new G.Location(3, 0)
                    )
                });

            IConfiguration configuration = new Configuration();
            configuration.DisableComments();

            var mapper = this.factory.CreateMapper(configuration);

            var result = mapper.MapToFeature(gherkinDocument);
            var scenario = result.FeatureElements[0];

            Check.That(scenario.Steps[0].Comments.Count).IsEqualTo(0);
            Check.That(scenario.Steps[1].Comments.Count).IsEqualTo(0);
            Check.That(scenario.Steps[2].Comments.Count).IsEqualTo(0);
            Check.That(scenario.Steps[3].Comments.Count).IsEqualTo(0);
        }

        [Test]
        public void MapToFeature_FeatureWithoutLanguage_ReturnsFeatureWithEnglish()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument("My Feature", null);

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.Language).Equals("en");
        }

        [Test]
        public void MapToFeature_FeatureWithDutchLanguage_ReturnsFeatureWithDutch()
        {
            var gherkinDocument = this.factory.CreateGherkinDocument("My Feature", null, language: "nl");

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToFeature(gherkinDocument);

            Check.That(result.Language).Equals("nl");
        }
    }
}