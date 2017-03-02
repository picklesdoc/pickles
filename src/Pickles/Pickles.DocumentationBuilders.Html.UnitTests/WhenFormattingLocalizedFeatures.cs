//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingLocalizedFeatures.cs" company="PicklesDoc">
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

using System.IO;
using Autofac;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests
{
    [TestFixture]
    public class WhenFormattingLocalizedFeatures : BaseFixture
    {
        [Test]
        public void ThenShouldLocalizeExamplesKeyword()
        {
            string featureText =
@"# language: nl-NL
Functionaliteit: Test het abstract scenario

Abstract Scenario: Het Scenario
    Stel dat ik 50 ingeef
    En dat ik 70 ingeef
    Als ik plus druk
    Dan moet het resultaat 120 zijn

Voorbeelden:
    | a |
    | 1 |
    | 2 |
";

            var parser = new FeatureParser(FileSystem, this.Configuration);
            Feature feature = parser.Parse(new StringReader(featureText));

            var formatter = Container.Resolve<HtmlFeatureFormatter>();
            var output = formatter.Format(feature);

            var value = output.ToString();
            Check.That(value).Contains("Voorbeelden");

        }
    }
}
