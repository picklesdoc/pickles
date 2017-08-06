//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CultureAwareDialectProviderTests.cs" company="PicklesDoc">
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

using System.Collections.Generic;
using Gherkin;
using Gherkin.Ast;
using NFluent;
using NUnit.Framework;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class CultureAwareDialectProviderTests
    {
        [Test]
        public void GetDialect_WithSimpleLanguage_ReturnsThatLanguage()
        {
            var provider = CreateCultureAwareDialectProvider();

            var dialect = provider.GetDialect("nl", new Location());

            Check.That(dialect.Language).IsEqualTo("nl");
        }

        private static TestableCultureAwareDialectProvider CreateCultureAwareDialectProvider()
        {
            var provider = new TestableCultureAwareDialectProvider("en");
            return provider;
        }

        [Test]
        public void GetDialect_WithLanguageAndCulture_ReturnsThatLanguage()
        {
            var provider = CreateCultureAwareDialectProvider();

            var dialect = provider.GetDialect("nl-BE", new Location());

            Check.That(dialect.Language).IsEqualTo("nl");
        }

        [Test]
        public void GetDialect_WithLanguageThatIncludesHyphen_ReturnsThatLanguage()
        {
            var provider = CreateCultureAwareDialectProvider();

            var dialect = provider.GetDialect("en-lol", new Location());

            Check.That(dialect.Language).IsEqualTo("en-lol");
        }

        private class TestableCultureAwareDialectProvider : CultureAwareDialectProvider
        {
            internal new GherkinDialect GetDialect(string language, Location location)
            {
                return base.GetDialect(language, location);
            }

            public TestableCultureAwareDialectProvider(string defaultLanguage) : base(defaultLanguage)
            {
            }
        }
    }
}