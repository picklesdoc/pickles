//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenDeterminingTheSignatureOfAnNUnitExampleRow.cs" company="PicklesDoc">
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

using System.Text.RegularExpressions;


using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;
using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit2;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.NUnit.NUnit2
{
    [TestFixture]
    public class WhenDeterminingTheSignatureOfAnNUnitExampleRow : BaseFixture
    {
        [Test]
        public void ThenCanSuccessfullyMatch()
        {
            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers" };
            var exampleRow = new[] { "40", "50", "90" };

            var signatureBuilder = new NUnit2ExampleSignatureBuilder();
            Regex signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var isMatch = signature.IsMatch("Pickles.TestHarness.AdditionFeature.AddingSeveralNumbers(\"40\",\"50\",\"90\",System.String[])".ToLowerInvariant());
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void ThenCanSuccessfullyMatchExamplesWithLongValues()
        {
            var scenarioOutline = new ScenarioOutline { Name = "Deal correctly with overlong example values" };
            var exampleRow = new[]
            {
                "Please enter a valid two letter country code (e.g. DE)!",
                "This is just a very very very veery long error message!"
            };

            var signatureBuilder = new NUnit2ExampleSignatureBuilder();
            var signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var isMatch = signature.IsMatch("Pickles.TestHarness.nunit.ScenarioOutlinesFeature.DealCorrectlyWithOverlongExampleValues(\"Please enter a valid two letter country code (e.g. DE)!\",\"This is just a very very very veery long error message!\",null)".ToLowerInvariant());
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void ThenCanSuccessfullyMatchSpecialCharacters()
        {
            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers (foo-bar, foo bar)" };
            var exampleRow = new[] { "40", "50", "90" };

            var signatureBuilder = new NUnit2ExampleSignatureBuilder();
            Regex signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var isMatch = signature.IsMatch("Pickles.TestHarness.AdditionFeature.AddingSeveralNumbersFoo_BarFooBar(\"40\",\"50\",\"90\",System.String[])".ToLowerInvariant());
            Check.That(isMatch).IsTrue();
        }
    }
}
