//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingFeatureFiles.cs" company="PicklesDoc">
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

using Autofac;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.ObjectModel;
using StringReader = System.IO.StringReader;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class WhenParsingFeatureFiles : BaseFixture
    {
        private const string DocStringDelimiter = "\"\"\"";

        [Test]
        public void ThenCanParseFeatureWithMultipleScenariosSuccessfully()
        {
            string featureText =
                @"# ignore this comment
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

  Scenario: A scenario
    Given some feature
    When it runs
    Then I should see that this thing happens

    Scenario: Another scenario
    Given some other feature
    When it runs
    Then I should see that this other thing happens
        And something else";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature).IsNotNull();
            Check.That(feature.Name).IsEqualTo("Test");
            Check.That(feature.Description.ComparisonNormalize()).IsEqualTo(@"    In order to do something
    As a user
    I want to run this scenario".ComparisonNormalize());
            Check.That(feature.FeatureElements.Count).IsEqualTo(2);
            Check.That(feature.Tags).IsEmpty();

            IFeatureElement scenario = feature.FeatureElements[0];
            Check.That(scenario.Name).IsEqualTo("A scenario");
            Check.That(scenario.Description).IsEqualTo("");
            Check.That(scenario.Steps.Count).IsEqualTo(3);
            Check.That(scenario.Tags).IsEmpty();

            Step givenStep = scenario.Steps[0];
            Check.That(givenStep.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep.Name).IsEqualTo("some feature");
            Check.That(givenStep.DocStringArgument).IsNull();
            Check.That(givenStep.TableArgument).IsNull();

            Step whenStep = scenario.Steps[1];
            Check.That(whenStep.Keyword).IsEqualTo(Keyword.When);
            Check.That(whenStep.Name).IsEqualTo("it runs");
            Check.That(whenStep.DocStringArgument).IsNull();
            Check.That(whenStep.TableArgument).IsNull();

            Step thenStep = scenario.Steps[2];
            Check.That(thenStep.Keyword).IsEqualTo(Keyword.Then);
            Check.That(thenStep.Name).IsEqualTo("I should see that this thing happens");
            Check.That(thenStep.DocStringArgument).IsNull();
            Check.That(thenStep.TableArgument).IsNull();

            IFeatureElement scenario2 = feature.FeatureElements[1];
            Check.That(scenario2.Name).IsEqualTo("Another scenario");
            Check.That(scenario2.Description).IsEqualTo(string.Empty);
            Check.That(scenario2.Steps.Count).IsEqualTo(4);
            Check.That(scenario2.Tags).IsEmpty();

            Step givenStep2 = scenario2.Steps[0];
            Check.That(givenStep2.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep2.Name).IsEqualTo("some other feature");
            Check.That(givenStep2.DocStringArgument).IsNull();
            Check.That(givenStep2.TableArgument).IsNull();

            Step whenStep2 = scenario2.Steps[1];
            Check.That(whenStep2.Keyword).IsEqualTo(Keyword.When);
            Check.That(whenStep2.Name).IsEqualTo("it runs");
            Check.That(whenStep2.DocStringArgument).IsNull();
            Check.That(whenStep2.TableArgument).IsNull();

            Step thenStep2 = scenario2.Steps[2];
            Check.That(thenStep2.Keyword).IsEqualTo(Keyword.Then);
            Check.That(thenStep2.Name).IsEqualTo("I should see that this other thing happens");
            Check.That(thenStep2.DocStringArgument).IsNull();
            Check.That(thenStep2.TableArgument).IsNull();

            Step thenStep3 = scenario2.Steps[3];
            Check.That(thenStep3.Keyword).IsEqualTo(Keyword.And);
            Check.That(thenStep3.Name).IsEqualTo("something else");
            Check.That(thenStep3.DocStringArgument).IsNull();
            Check.That(thenStep3.TableArgument).IsNull();
        }

        [Test]
        public void ThenCanParseMostBasicFeatureSuccessfully()
        {
            string featureText =
                @"# ignore this comment
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

  Scenario: A scenario
    Given some feature
    When it runs
    Then I should see that this thing happens";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature).IsNotNull();
            Check.That(feature.Name).IsEqualTo("Test");
            Check.That(feature.Description.ComparisonNormalize()).IsEqualTo(@"    In order to do something
    As a user
    I want to run this scenario".ComparisonNormalize());
            Check.That(feature.FeatureElements.Count).IsEqualTo(1);
            Check.That(feature.Tags).IsEmpty();

            IFeatureElement scenario = feature.FeatureElements.First();
            Check.That(scenario.Name).IsEqualTo("A scenario");
            Check.That(scenario.Description).IsEqualTo(string.Empty);
            Check.That(scenario.Steps.Count).IsEqualTo(3);
            Check.That(scenario.Tags).IsEmpty();

            Step givenStep = scenario.Steps[0];
            Check.That(givenStep.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep.Name).IsEqualTo("some feature");
            Check.That(givenStep.DocStringArgument).IsNull();
            Check.That(givenStep.TableArgument).IsNull();

            Step whenStep = scenario.Steps[1];
            Check.That(whenStep.Keyword).IsEqualTo(Keyword.When);
            Check.That(whenStep.Name).IsEqualTo("it runs");
            Check.That(whenStep.DocStringArgument).IsNull();
            Check.That(whenStep.TableArgument).IsNull();

            Step thenStep = scenario.Steps[2];
            Check.That(thenStep.Keyword).IsEqualTo(Keyword.Then);
            Check.That(thenStep.Name).IsEqualTo("I should see that this thing happens");
            Check.That(thenStep.DocStringArgument).IsNull();
            Check.That(thenStep.TableArgument).IsNull();
        }

        [Test]
        public void ThenCanParseScenarioOutlinesSuccessfully()
        {
            string featureText =
                @"# ignore this comment
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

  Scenario Outline: A scenario outline
    Given some feature with <keyword1>
    When it runs
    Then I should see <keyword2>

    Examples:
    | keyword1 | keyword2 |
    | this     | that     |";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            var scenarioOutline = feature.FeatureElements[0] as ScenarioOutline;
            Check.That(scenarioOutline).IsNotNull();
            Check.That(scenarioOutline.Name).IsEqualTo("A scenario outline");
            Check.That(scenarioOutline.Description).IsEqualTo(string.Empty);
            Check.That(scenarioOutline.Steps.Count).IsEqualTo(3);

            Step givenStep = scenarioOutline.Steps[0];
            Check.That(givenStep.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep.Name).IsEqualTo("some feature with <keyword1>");
            Check.That(givenStep.DocStringArgument).IsNull();
            Check.That(givenStep.TableArgument).IsNull();

            Step whenStep = scenarioOutline.Steps[1];
            Check.That(whenStep.Keyword).IsEqualTo(Keyword.When);
            Check.That(whenStep.Name).IsEqualTo("it runs");
            Check.That(whenStep.DocStringArgument).IsNull();
            Check.That(whenStep.TableArgument).IsNull();

            Step thenStep = scenarioOutline.Steps[2];
            Check.That(thenStep.Keyword).IsEqualTo(Keyword.Then);
            Check.That(thenStep.Name).IsEqualTo("I should see <keyword2>");
            Check.That(thenStep.DocStringArgument).IsNull();
            Check.That(thenStep.TableArgument).IsNull();

            var examples = scenarioOutline.Examples;
            Check.That(examples.First().Name).IsNullOrEmpty();
            Check.That(examples.First().Description).IsNullOrEmpty();

            Table table = examples.First().TableArgument;
            Check.That(table.HeaderRow.Cells[0]).IsEqualTo("keyword1");
            Check.That(table.HeaderRow.Cells[1]).IsEqualTo("keyword2");
            Check.That(table.DataRows[0].Cells[0]).IsEqualTo("this");
            Check.That(table.DataRows[0].Cells[1]).IsEqualTo("that");
        }

        [Test]
        public void ThenCanParseScenarioWithBackgroundSuccessfully()
        {
            string featureText =
                @"# ignore this comment
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

    Background: Some background for the scenarios
    Given some prior context
        And yet more prior context

  Scenario: A scenario
    Given some feature
    When it runs
    Then I should see that this thing happens";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature.Background).IsNotNull();
            Check.That(feature.Background.Name).IsEqualTo("Some background for the scenarios");
            Check.That(feature.Background.Description).IsEqualTo(string.Empty);
            Check.That(feature.Background.Steps.Count).IsEqualTo(2);
            Check.That(feature.Background.Tags).IsEmpty();

            Step givenStep1 = feature.Background.Steps[0];
            Check.That(givenStep1.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep1.Name).IsEqualTo("some prior context");
            Check.That(givenStep1.DocStringArgument).IsNull();
            Check.That(givenStep1.TableArgument).IsNull();

            Step givenStep2 = feature.Background.Steps[1];
            Check.That(givenStep2.Keyword).IsEqualTo(Keyword.And);
            Check.That(givenStep2.Name).IsEqualTo("yet more prior context");
            Check.That(givenStep2.DocStringArgument).IsNull();
            Check.That(givenStep2.TableArgument).IsNull();
        }

        [Test]
        public void ThenCanParseScenarioWithDocstringSuccessfully()
        {
            string docstring = string.Format(
                @"{0}
This is a document string
it can be many lines long
{0}",
                DocStringDelimiter);

            string featureText =
                string.Format(
                    @"Feature: Test
    In order to do something
    As a user
    I want to run this scenario

    Scenario: A scenario
        Given some feature
        {0}
        When it runs
        Then I should see that this thing happens",
                    docstring);

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature.FeatureElements[0].Steps[0].DocStringArgument.ComparisonNormalize()).IsEqualTo(@"This is a document string
it can be many lines long".ComparisonNormalize());
        }

        [Test]
        public void ThenCanParseScenarioWithTableSuccessfully()
        {
            string featureText =
                @"# ignore this comment
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

  Scenario: A scenario
    Given some feature with a table
        | Column1 | Column2 |
        | Value 1 | Value 2 |
    When it runs
    Then I should see that this thing happens";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            Table table = feature.FeatureElements[0].Steps[0].TableArgument;
            Check.That(table.HeaderRow.Cells[0]).IsEqualTo("Column1");
            Check.That(table.HeaderRow.Cells[1]).IsEqualTo("Column2");
            Check.That(table.DataRows[0].Cells[0]).IsEqualTo("Value 1");
            Check.That(table.DataRows[0].Cells[1]).IsEqualTo("Value 2");
        }

        [Test]
        public void Then_can_parse_scenario_with_tags_successfully()
        {
            string featureText =
                @"# ignore this comment
@feature-tag
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

    @scenario-tag-1 @scenario-tag-2
  Scenario: A scenario
    Given some feature
    When it runs
    Then I should see that this thing happens";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature.Tags[0]).IsEqualTo("@feature-tag");
            Check.That(feature.FeatureElements[0].Tags[0]).IsEqualTo("@scenario-tag-1");
            Check.That(feature.FeatureElements[0].Tags[1]).IsEqualTo("@scenario-tag-2");
        }

        [Test]
        public void Then_can_parse_scenario_with_comments_successfully()
        {
            string featureText =
              @"# ignore this comment
Feature: Test
    In order to do something
    As a user
    I want to run this scenario

  Scenario: A scenario
    # A single line comment
    Given some feature
    # A multiline comment - first line
    # Second line
    When it runs
    Then I should see that this thing happens
    # A last comment after the scenario";

            var parser = Container.Resolve<FeatureParser>();
            Feature feature = parser.Parse(new StringReader(featureText));

            IFeatureElement scenario = feature.FeatureElements.First();

            Step stepGiven = scenario.Steps[0];
            Check.That(stepGiven.Comments.Count).IsEqualTo(1);
            Check.That(stepGiven.Comments[0].Text).IsEqualTo("# A single line comment");

            Step stepWhen = scenario.Steps[1];
            Check.That(stepWhen.Comments.Count).IsEqualTo(2);
            Check.That(stepWhen.Comments[0].Text).IsEqualTo("# A multiline comment - first line");
            Check.That(stepWhen.Comments[1].Text).IsEqualTo("# Second line");

            Step stepThen = scenario.Steps[2];
            Check.That(stepThen.Comments.Count).IsEqualTo(1);
            Check.That(stepThen.Comments.Count(o => o.Type == CommentType.StepComment)).IsEqualTo(0);
            Check.That(stepThen.Comments.Count(o => o.Type == CommentType.AfterLastStepComment)).IsEqualTo(1);
            Check.That(stepThen.Comments[0].Text = "# A last comment after the scenario");
        }


    }
}
