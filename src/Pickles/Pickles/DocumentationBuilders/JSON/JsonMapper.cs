//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="JsonMapper.cs" company="PicklesDoc">
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
using AutoMapper;
using AutoMapper.Mappers;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON
{
    public class JsonMapper : IDisposable
    {
        private readonly MappingEngine mapper;

        public JsonMapper()
        {
            var configurationStore = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);

            this.mapper = new MappingEngine(configurationStore);

            configurationStore.CreateMap<Feature, JsonFeature>()
                .ForMember(t => t.FeatureElements, opt => opt.ResolveUsing(s => s.FeatureElements))
                .AfterMap(
                    (sourceFeature, targetFeature) =>
                        {
                            foreach (var featureElement in targetFeature.FeatureElements.ToArray())
                            {
                                featureElement.Feature = targetFeature;
                            }
                        });
            configurationStore.CreateMap<Example, JsonExample>();
            configurationStore.CreateMap<Keyword, JsonKeyword>();
            configurationStore.CreateMap<Scenario, JsonScenario>()
                 .ForMember(t => t.Feature, opt => opt.Ignore());
            configurationStore.CreateMap<ScenarioOutline, JsonScenarioOutline>()
                 .ForMember(t => t.Feature, opt => opt.Ignore());
            configurationStore.CreateMap<Comment, JsonComment>();
            configurationStore.CreateMap<Step, JsonStep>()
                .ForMember(t => t.StepComments, opt => opt.UseValue(new List<JsonComment>()))
                .ForMember(t => t.AfterLastStepComments, opt => opt.UseValue(new List<JsonComment>()))
                .AfterMap(
                    (sourceStep, targetStep) =>
                    {
                        this.mapper.Map(sourceStep.Comments.Where(o => o.Type == CommentType.StepComment), targetStep.StepComments);
                        this.mapper.Map(sourceStep.Comments.Where(o => o.Type == CommentType.AfterLastStepComment), targetStep.AfterLastStepComments);
                    }
                );
            configurationStore.CreateMap<Table, JsonTable>();
            configurationStore.CreateMap<TestResult, JsonTestResult>().ConstructUsing(ToJsonTestResult);

            configurationStore.CreateMap<TableRow, JsonTableRow>()
                .ConstructUsing(row => new JsonTableRow(row.Cells.ToArray()));

            configurationStore.CreateMap<IFeatureElement, IJsonFeatureElement>().ConvertUsing(
                sd =>
                {
                    var scenario = sd as Scenario;
                    if (scenario != null)
                    {
                        return this.mapper.Map<JsonScenario>(scenario);
                    }

                    var scenarioOutline = sd as ScenarioOutline;
                    if (scenarioOutline != null)
                    {
                        return this.mapper.Map<JsonScenarioOutline>(scenarioOutline);
                    }

                    throw new ArgumentException("Only arguments of type Scenario and ScenarioOutline are supported.");
                });
        }

        public JsonTableRow Map(TableRow tableRow)
        {
            return this.mapper.Map<JsonTableRow>(tableRow);
        }

        public JsonFeature Map(Feature feature)
        {
            return this.mapper.Map<JsonFeature>(feature);
        }

        public JsonTestResult Map(TestResult testResult)
        {
            return this.mapper.Map<JsonTestResult>(testResult);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.mapper.Dispose();
            }
        }

        private static JsonTestResult ToJsonTestResult(TestResult testResult)
        {
            switch (testResult)
            {
                case TestResult.Failed:
                    return new JsonTestResult { WasExecuted = true, WasSuccessful = false };
                case TestResult.Passed:
                    return new JsonTestResult { WasExecuted = true, WasSuccessful = true };
                default:
                    return new JsonTestResult { WasExecuted = false, WasSuccessful = false };
            }
        }
    }
}
