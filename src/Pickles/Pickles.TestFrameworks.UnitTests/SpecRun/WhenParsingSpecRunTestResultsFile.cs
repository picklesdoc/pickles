//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingSpecRunTestResultsFile.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.TestFrameworks.SpecRun;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.SpecRun
{
    [TestFixture]
    public class WhenParsingSpecRunTestResultsFile : StandardTestSuite<SpecRunResults>
    {
        public WhenParsingSpecRunTestResultsFile()
            : base("SpecRun." + "results-example-specrun.html")
        {
        }

        [Test]
        public new void ThenCanReadBackgroundResultSuccessfully()
        {
            base.ThenCanReadBackgroundResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadInconclusiveFeatureResultSuccessfully()
        {
            base.ThenCanReadInconclusiveFeatureResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadFailedFeatureResultSuccessfully()
        {
            base.ThenCanReadFailedFeatureResultSuccessfully();
        }

        [Test]
        public new void ThenCanReadPassedFeatureResultSuccessfully()
        {
            base.ThenCanReadPassedFeatureResultSuccessfully();
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
        public new void ThenCanReadInconclusiveScenarioResultSuccessfully()
        {
            base.ThenCanReadInconclusiveScenarioResultSuccessfully();
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


        //SpecRun-Output does not encode & so it can not be read as xml
        //[Test]
        //public new void ThenCanReadResultOfScenarioOutlineWithAmpersand()
        //{
        //  base.ThenCanReadResultOfScenarioOutlineWithAmpersand();
        //}

        [Test]
        public new void ThenCanReadResultOfScenarioWithSpanishCharacters()
        {
            base.ThenCanReadResultOfScenarioWithSpanishCharacters();
        }
    }
}
