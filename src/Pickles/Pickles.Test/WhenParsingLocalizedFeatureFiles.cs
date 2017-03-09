//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingLocalizedFeatureFiles.cs" company="PicklesDoc">
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
    public class WhenParsingLocalizedFeatureFiles : BaseFixture
    {
        [Test]
        public void WhenIndicatingTheLanguageInTheFeature_ThenCanParseMostBasicFeatureSuccessfully()
        {
            string featureText =
                @"# language: sv
# ignorera denna kommentar
Egenskap: Test egenskap
  Som svensk användare
  Vill jag skriva mina krav på svenska
  Så att beställaren kan förstå dem

  Scenario: Ett scenario
        Givet en egenskap
        När den körs
        Så skall jag se att det inträffat";

            var configuration = this.Configuration;
            var parser = this.CreateParser(configuration);
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature).IsNotNull();
            Check.That(feature.Name).IsEqualTo("Test egenskap");
            Check.That(feature.Description.ComparisonNormalize()).IsEqualTo(@"Som svensk användare
Vill jag skriva mina krav på svenska
Så att beställaren kan förstå dem".ComparisonNormalize());
            Check.That(feature.FeatureElements.Count).IsEqualTo(1);
            Check.That(feature.Tags.Count).IsEqualTo(0);

            IFeatureElement scenario = feature.FeatureElements.First();
            Check.That(scenario.Name).IsEqualTo("Ett scenario");
            Check.That(scenario.Description).IsEqualTo(string.Empty);
            Check.That(scenario.Steps.Count).IsEqualTo(3);
            Check.That(scenario.Tags.Count).IsEqualTo(0);

            Step givenStep = scenario.Steps[0];
            Check.That(givenStep.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep.Name).IsEqualTo("en egenskap");
            Check.That(givenStep.DocStringArgument).IsNull();
            Check.That(givenStep.TableArgument).IsNull();

            Step whenStep = scenario.Steps[1];
            Check.That(whenStep.Keyword).IsEqualTo(Keyword.When);
            Check.That(whenStep.Name).IsEqualTo("den körs");
            Check.That(whenStep.DocStringArgument).IsNull();
            Check.That(whenStep.TableArgument).IsNull();

            Step thenStep = scenario.Steps[2];
            Check.That(thenStep.Keyword).IsEqualTo(Keyword.Then);
            Check.That(thenStep.Name).IsEqualTo("skall jag se att det inträffat");
            Check.That(thenStep.DocStringArgument).IsNull();
            Check.That(thenStep.TableArgument).IsNull();
        }

        private FeatureParser CreateParser(IConfiguration configuration)
        {
            return new FeatureParser(configuration);
        }

        [Test]
        public void WhenSettingTheLanguageInTheConfiguration_ThenCanParseMostBasicFeatureSuccessfully()
        {
            string featureText =
                @"# ignorera denna kommentar
Egenskap: Test egenskap
  Som svensk användare
  Vill jag skriva mina krav på svenska
  Så att beställaren kan förstå dem

  Scenario: Ett scenario
        Givet en egenskap
        När den körs
        Så skall jag se att det inträffat";

            var configuration = this.Configuration;

            configuration.Language = "sv";

            var parser = this.CreateParser(configuration);
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature).IsNotNull();
            Check.That(feature.Name).IsEqualTo("Test egenskap");
            Check.That(feature.Description.ComparisonNormalize()).IsEqualTo(@"Som svensk användare
Vill jag skriva mina krav på svenska
Så att beställaren kan förstå dem".ComparisonNormalize());
            Check.That(feature.FeatureElements.Count).IsEqualTo(1);
            Check.That(feature.Tags.Count).IsEqualTo(0);

            IFeatureElement scenario = feature.FeatureElements.First();
            Check.That(scenario.Name).IsEqualTo("Ett scenario");
            Check.That(scenario.Description).IsEqualTo(string.Empty);
            Check.That(scenario.Steps.Count).IsEqualTo(3);
            Check.That(scenario.Tags.Count).IsEqualTo(0);

            Step givenStep = scenario.Steps[0];
            Check.That(givenStep.Keyword).IsEqualTo(Keyword.Given);
            Check.That(givenStep.Name).IsEqualTo("en egenskap");
            Check.That(givenStep.DocStringArgument).IsNull();
            Check.That(givenStep.TableArgument).IsNull();

            Step whenStep = scenario.Steps[1];
            Check.That(whenStep.Keyword).IsEqualTo(Keyword.When);
            Check.That(whenStep.Name).IsEqualTo("den körs");
            Check.That(whenStep.DocStringArgument).IsNull();
            Check.That(whenStep.TableArgument).IsNull();

            Step thenStep = scenario.Steps[2];
            Check.That(thenStep.Keyword).IsEqualTo(Keyword.Then);
            Check.That(thenStep.Name).IsEqualTo("skall jag se att det inträffat");
            Check.That(thenStep.DocStringArgument).IsNull();
            Check.That(thenStep.TableArgument).IsNull();
        }

        [Test]
        public void WhenIndicatingTheLanguageAndCultureInTheFeature_ThenCanParseTheFeature()
        {
            string featureText =
       @"# language: nl-BE
Functionaliteit: Test de Cultuur

Scenario: Het Scenario
    Stel dat ik 50 ingeef
    En dat ik 70 ingeef
    Als ik plus druk
    Dan moet het resultaat 120 zijn";

            var configuration = this.Configuration;
            var parser = this.CreateParser(configuration);
            Feature feature = parser.Parse(new StringReader(featureText));

            Check.That(feature).IsNotNull();
        }
    }
}
