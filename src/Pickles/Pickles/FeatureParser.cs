//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureParser.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.ObjectModel;

using TextReader = System.IO.TextReader;

namespace PicklesDoc.Pickles
{
    public class FeatureParser
    {
        private readonly IConfiguration configuration;

        private readonly DescriptionProcessor descriptionProcessor = new DescriptionProcessor();

        private readonly LanguageServicesRegistry languageServicesRegistry = new LanguageServicesRegistry();

        private readonly IDictionary<string, IGherkinDialectProvider> dialectProviderCache = new Dictionary<string, IGherkinDialectProvider>();

        public FeatureParser(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Feature Parse(TextReader featureFileReader)
        {
            var language = this.DetermineLanguage();
            var gherkinParser = new Gherkin.Parser();
            var dialectProvider = this.GetDialectProvider(language);

            try
            {
                Gherkin.Ast.GherkinDocument gherkinDocument = gherkinParser.Parse(
                    new Gherkin.TokenScanner(featureFileReader),
                    new Gherkin.TokenMatcher(dialectProvider));

                var languageServices = this.languageServicesRegistry.GetLanguageServicesForLanguage(gherkinDocument.Feature.Language);
                Feature result = new Mapper(this.configuration, languageServices).MapToFeature(gherkinDocument);
                result = new FeatureFilter(result, this.configuration.ExcludeTags).ExcludeScenariosByTags();

                if (result != null)
                    this.descriptionProcessor.Process(result);

                return result;
            }
            catch (Gherkin.CompositeParserException exception)
            {
                throw new FeatureParseException("Unable to parse feature", exception);
            }
        }

        private IGherkinDialectProvider GetDialectProvider(string language)
        {
            IGherkinDialectProvider dialectProvider;
            if (this.dialectProviderCache.ContainsKey(language))
            {
                dialectProvider = this.dialectProviderCache[language];
            }
            else
            {
                dialectProvider = new CultureAwareDialectProvider(language);
                this.dialectProviderCache[language] = dialectProvider;
            }

            return dialectProvider;
        }

        private string DetermineLanguage()
        {
            string language = null;

            if (!string.IsNullOrWhiteSpace(this.configuration.Language))
            {
                language = this.configuration.Language;
            }
            return language;
        }
    }
}
