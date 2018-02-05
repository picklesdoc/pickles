//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LanguageServicesRegistry.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles
{
    public class LanguageServicesRegistry : ILanguageServicesRegistry
    {
        private static readonly IDictionary<string, ILanguageServices> LanguageServices = new Dictionary<string, ILanguageServices>();

        public ILanguageServices GetLanguageServicesForLanguage(string language)
        {
            language = language ?? DefaultLanguage;

            if (LanguageServices.ContainsKey(language))
            {
                return LanguageServices[language];
            }

            var services = new LanguageServices(language);
            LanguageServices[language] = services;
            return services;
        }

        public string DefaultLanguage
        {
            get { return "en"; }
        }
    }
}