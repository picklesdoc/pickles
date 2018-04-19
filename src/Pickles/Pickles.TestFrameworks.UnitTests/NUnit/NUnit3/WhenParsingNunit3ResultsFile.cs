//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingNunit3ResultsFile.cs" company="PicklesDoc">
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

using NUnit.Framework;

using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit3;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.NUnit.NUnit3
{
    [TestFixture]
    public class WhenParsingNunit3ResultsFile : StandardTestSuite<NUnit3Results>
    {
        public WhenParsingNunit3ResultsFile()
            : base("NUnit.NUnit3." + "results-example-nunit3.xml")
        {
        }

        [Test]
        public new void ThenCanReadFeatureResultSuccessfully()
        {
            base.ThenCanReadFeatureResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadScenarioOutlineResultSuccessfully()
        {
            base.ThenCanReadScenarioOutlineResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadSuccessfulScenarioResultSuccessfully()
        {
            base.ThenCanReadSuccessfulScenarioResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadFailedScenarioResultSuccessfully()
        {
            base.ThenCanReadFailedScenarioResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadIgnoredScenarioResultSuccessfully()
        {
            base.ThenCanReadIgnoredScenarioResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadNotFoundScenarioCorrectly()
        {
            base.ThenCanReadNotFoundScenarioCorrectly();
        }

        [Test]
        public new void ThenCanReadResultsWithBackslashes()
        {
            base.ThenCanReadResultsWithBackslashes();
        }

        [Test]
        public new void ThenCanReadResultsWithParenthesis()
        {
            base.ThenCanReadResultsWithParenthesis();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioWithFailingBackground()
        {
            base.ThenCanReadResultOfScenarioWithFailingBackground();
        }

        [Test]
        public new void ThenCanReadResultOfFeatureWithFailingBackground()
        {
            base.ThenCanReadResultOfFeatureWithFailingBackground();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioOutlineWithFailingBackground()
        {
            base.ThenCanReadResultOfScenarioOutlineWithFailingBackground();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioOutlineExampleWithFailingBackground()
        {
            base.ThenCanReadResultOfScenarioOutlineExampleWithFailingBackground();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioWithSpecialCharacters()
        {
            base.ThenCanReadResultOfScenarioWithSpecialCharacters();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioOutlineWithSpecialCharacters()
        {
            base.ThenCanReadResultOfScenarioOutlineWithSpecialCharacters();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioOutlineWithUmlauts()
        {
          base.ThenCanReadResultOfScenarioOutlineWithUmlauts();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioWithDanishCharacters()
        {
            base.ThenCanReadResultOfScenarioWithDanishCharacters();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioOutlineWithAmpersand()
        {
          base.ThenCanReadResultOfScenarioOutlineWithAmpersand();
        }

        [Test]
        public new void ThenCanReadResultOfScenarioWithSpanishCharacters()
        {
            base.ThenCanReadResultOfScenarioWithSpanishCharacters();
        }
    }
}
