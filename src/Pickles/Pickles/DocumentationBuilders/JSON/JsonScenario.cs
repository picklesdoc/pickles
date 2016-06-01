//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="JsonScenario.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON
{
    public class JsonScenario : IJsonFeatureElement
    {
        public JsonScenario()
        {
            this.Steps = new List<JsonStep>();
            this.Tags = new List<string>();
        }

        #region IFeatureElement Members

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public List<JsonStep> Steps { get; set; }

        public List<string> Tags { get; set; }

        public JsonTestResult Result { get; set; }

        public JsonFeature Feature { get; set; }

        #endregion
    }
}
