//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExampleToJsonExampleMapper.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.Linq;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper
{
    public class ExampleToJsonExampleMapper
    {
        private readonly TableToJsonTableMapper tableMapper;
        private readonly ILanguageServicesRegistry languageServicesRegistry;

        public ExampleToJsonExampleMapper(ILanguageServicesRegistry languageServicesRegistry)
        {
            this.languageServicesRegistry = languageServicesRegistry;
            this.tableMapper = new TableToJsonTableMapper();
        }

        public JsonExample Map(Example example, string language)
        {
            if (example == null)
            {
                return null;
            }

            var languageServices = this.languageServicesRegistry.GetLanguageServicesForLanguage(language);

            var examplesKeyword = languageServices.ExamplesKeywords[0];

            return new JsonExample
            {
                Name = example.Name,
                Description = example.Description,
                TableArgument = this.tableMapper.MapWithTestResults(example.TableArgument),
                Tags = (example.Tags ?? new List<string>()).ToList(),
                NativeKeyword = examplesKeyword
            };
        }
    }
}