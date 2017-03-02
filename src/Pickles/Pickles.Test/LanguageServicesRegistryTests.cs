//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LanguageServicesRegistryTests.cs" company="PicklesDoc">
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

using NFluent;
using NUnit.Framework;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class LanguageServicesRegistryTests
    {
        [Test]
        public void GetLanguageServicesForLanguage_WithEnglish_ReturnsLanguageServicesForEnglish()
        {
            var languageServicesRegistry = new LanguageServicesRegistry();

            ILanguageServices languageServices = languageServicesRegistry.GetLanguageServicesForLanguage("en");

            Check.That(languageServices.Language).IsEqualTo("en");
        }

        [Test]
        public void DefaultLanguage_ReturnsEnglish()
        {
            var languageServicesRegistry = new LanguageServicesRegistry();

            string defaultLanguage = languageServicesRegistry.DefaultLanguage;

            Check.That(defaultLanguage).IsEqualTo("en");
        }

        [Test]
        public void GetLanguageServicesForLanguage_WithNullLanguage_ReturnsLanguageServicesForEnglish()
        {
            var languageServicesRegistry = new LanguageServicesRegistry();

            ILanguageServices languageServices = languageServicesRegistry.GetLanguageServicesForLanguage(null);

            Check.That(languageServices.Language).IsEqualTo("en");
        }
    }
}