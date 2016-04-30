//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureToJsonFeatureMapper.cs" company="PicklesDoc">
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
using System.Linq;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON.Mapper
{
    public class FeatureToJsonFeatureMapper
    {
        private readonly ScenarioToJsonScenarioMapper scenarioMapper;
        private readonly ScenarioOutlineToJsonScenarioOutlineMapper scenarioOutlineMapper;
        private readonly TestResultToJsonTestResultMapper resultMapper;

        public FeatureToJsonFeatureMapper()
        {
            this.scenarioMapper = new ScenarioToJsonScenarioMapper();
            this.scenarioOutlineMapper = new ScenarioOutlineToJsonScenarioOutlineMapper();
            this.resultMapper = new TestResultToJsonTestResultMapper();
        }

        public JsonFeature Map(Feature feature)
        {
            if (feature == null)
            {
                return null;
            }

            var result = new JsonFeature
            {
                Name = feature.Name,
                Description = feature.Description,
                Result = this.resultMapper.Map(feature.Result),
                Tags = feature.Tags.ToArray().ToList(),
            };

            result.FeatureElements.AddRange(feature.FeatureElements.Select(this.MapFeatureElement).ToList());
            result.Background = this.scenarioMapper.Map(feature.Background);

            if (result.Background != null)
            {
                result.Background.Feature = result;
            }

            foreach (var featureElement in result.FeatureElements)
            {
                featureElement.Feature = result;
            }

            return result;
        }

        private IJsonFeatureElement MapFeatureElement(IFeatureElement arg)
        {
            var scenario = arg as Scenario;

            if (scenario != null)
            {
                return this.scenarioMapper.Map(scenario);
            }

            var scenarioOutline = arg as ScenarioOutline;

            if (scenarioOutline != null)
            {
                return this.scenarioOutlineMapper.Map(scenarioOutline);
            }

            throw new ArgumentException("Only arguments of type Scenario and ScenarioOutline are supported.");
        }
    }
}