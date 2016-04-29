//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioOutlineToJsonScenarioOutlineMapper.cs" company="PicklesDoc">
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
using System.Linq;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON.Mapper
{
    public class ScenarioOutlineToJsonScenarioOutlineMapper
    {
        private readonly TestResultToJsonTestResultMapper resultMapper;
        private readonly StepToJsonStepMapper stepMapper;
        private readonly ExampleToJsonExampleMapper exampleMapper;

        public ScenarioOutlineToJsonScenarioOutlineMapper()
        {
            this.resultMapper = new TestResultToJsonTestResultMapper();
            this.stepMapper = new StepToJsonStepMapper();
            this.exampleMapper = new ExampleToJsonExampleMapper();
        }

        public JsonScenarioOutline Map(ScenarioOutline scenarioOutline)
        {
            if (scenarioOutline == null)
            {
                return null;
            }

            return new JsonScenarioOutline
            {
                Examples = (scenarioOutline.Examples ?? new List<Example>()).Select(this.exampleMapper.Map).ToList(),
                Steps = (scenarioOutline.Steps ?? new List<Step>()).Select(this.stepMapper.Map).ToList(),
                Tags = (scenarioOutline.Tags ?? new List<string>()).ToList(),
                Name = scenarioOutline.Name,
                Description = scenarioOutline.Description,
                Result = this.resultMapper.Map(scenarioOutline.Result),
            };
        }
    }
}