//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CultureAwareDialectProvider.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles
{
    public class CultureAwareDialectProvider : GherkinDialectProvider
    {
        public CultureAwareDialectProvider(string defaultLanguage) : base(defaultLanguage)
        {
        }

        /// <remarks>We need to override only this method. The overload without
        /// Dictionary internally calls this method.</remarks>
        protected override GherkinDialect GetDialect(string language,
            Dictionary<string, GherkinLanguageSetting> gherkinLanguageSettings, Location location)
        {
            GherkinDialect result;

            try
            {
                result = base.GetDialect(language, gherkinLanguageSettings, location);
            }
            catch (NoSuchLanguageException)
            {
                string languageOnly = StripCulture(language);

                result = base.GetDialect(languageOnly, gherkinLanguageSettings, location);
            }

            return result;
        }

        private string StripCulture(string language)
        {
            if (language != null && language.Contains("-"))
            {
                return language.Split('-')[0];
            }

            return language;
        }
    }
}