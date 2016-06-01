//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioBase.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.ObjectModel
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    public abstract class ScenarioBase : IFeatureElement
    {
        protected ScenarioBase()
        {
            this.Steps = new List<Step>();
            this.Tags = new List<string>();
        }

        public List<Example> Examples { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public List<Step> Steps { get; set; }

        public List<string> Tags { get; set; }

        public TestResult Result { get; set; }

        public Feature Feature { get; set; }

        public Location Location { get; set; }
    }
}