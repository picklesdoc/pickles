//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenDeterminingTheSignatureOfAnXUnit2ExampleRow.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.TestFrameworks.XUnit;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.XUnit.XUnit2
{
    [TestFixture]
    public class WhenDeterminingTheSignatureOfAnXUnit2ExampleRow : BaseFixture
    {
        [Test]
        public void ThenCanSuccessfullyMatch()
        {
            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers" };
            var exampleRow = new[] { "40", "50", "90" };

            var signatureBuilder = new XUnitExampleSignatureBuilder();
            Regex signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var isMatch = signature.IsMatch("Pickles.TestHarness.xUnit.AdditionFeature.AddingSeveralNumbers(firstNumber: \"40\", secondNumber: \"50\", result: \"90\", exampleTags: System.String[])".ToLowerInvariant());
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

            var signatureBuilder = new XUnitExampleSignatureBuilder();
            var signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var isMatch = signature.IsMatch("Pickles.TestHarness.xunit2.ScenarioOutlinesFeature.DealCorrectlyWithOverlongExampleValues(value1: \"Please enter a valid two letter country code (e.g.\"..., value2: \"This is just a very very very veery long error mes\"..., exampleTags: [])".ToLowerInvariant());
            Check.That(isMatch).IsTrue();
        }

        private static bool MatchRegexSpecialChars(string expectedParameter)
        {
            var scenarioOutline = new ScenarioOutline { Name = "This scenario contains examples with Regex-special characters" };
            var exampleRow = new[] { expectedParameter };

            var signatureBuilder = new XUnitExampleSignatureBuilder();
            var signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var matchEntry = string
                .Format(
                    "Pickles.TestHarness.xunit2.ScenariosWithSpecialCharactersFeature.ThisScenarioContainsExamplesWithRegex_SpecialCharacters(regex: \"{0}\", exampleTags: System.String[])",
                    expectedParameter)
                .ToLowerInvariant();

            return signature.IsMatch(matchEntry);
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_Asterisk()
        {
            var isMatch = MatchRegexSpecialChars("**");
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_Plus()
        {
            var isMatch = MatchRegexSpecialChars("++");
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_FullStop_And_Asterisk()
        {
            var isMatch = MatchRegexSpecialChars(".*");
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_Angle_Brackets()
        {
            var isMatch = MatchRegexSpecialChars("[]");
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_Curly_Braces()
        {
            var isMatch = MatchRegexSpecialChars("{}");
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_Parentheses()
        {
            var isMatch = MatchRegexSpecialChars("()");
            Check.That(isMatch).IsTrue();
        }

        [Test]
        public void RegularExpressionAsExampleContentMatches_With_Full_Regular_Expression()
        {
            var isMatch = MatchRegexSpecialChars(@"^.*(?<foo>BAR)\s[^0-9]{3,4}A+$");
            Check.That(isMatch).IsTrue();
        }
    }
}
