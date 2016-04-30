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
using AutoMapper;
using AutoMapper.Mappers;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON
{
    public class JsonMapper : IDisposable
    {
        private readonly MappingEngine mapper;

        public JsonMapper()
        {
            var configurationStore = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);

            this.mapper = new MappingEngine(configurationStore);

            configurationStore.CreateMap<Feature, JsonFeature>().ConvertUsing(this.ToJsonFeature);
            configurationStore.CreateMap<Example, JsonExample>().ConvertUsing(this.ToJsonExample);
            configurationStore.CreateMap<Keyword, JsonKeyword>().ConvertUsing(this.ToJsonKeyword);
            configurationStore.CreateMap<Scenario, JsonScenario>().ConvertUsing(this.ToJsonScenario);
            configurationStore.CreateMap<ScenarioOutline, JsonScenarioOutline>().ConvertUsing(this.ToJsonScenarioOutline);
            configurationStore.CreateMap<Comment, JsonComment>().ConvertUsing(this.ToJsonComment);
            configurationStore.CreateMap<Step, JsonStep>().ConvertUsing(this.ToJsonStep);
            configurationStore.CreateMap<Table, JsonTable>().ConvertUsing(this.ToJsonTable);
            configurationStore.CreateMap<TestResult, JsonTestResult>().ConstructUsing(this.ToJsonTestResult);
            configurationStore.CreateMap<TableRow, JsonTableRow>().ConvertUsing(this.ToJsonTableRow);

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
            return this.ToJsonTestResult(testResult);
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

        private JsonTestResult ToJsonTestResult(TestResult testResult)
        {
            return new Mapper.TestResultToJsonTestResultMapper().Map(testResult);
        }

        private JsonComment ToJsonComment(Comment comment)
        {
            return new Mapper.CommentToJsonCommentMapper().Map(comment);
        }

        private JsonKeyword ToJsonKeyword(Keyword keyword)
        {
            return new Mapper.KeywordToJsonKeywordMapper().Map(keyword);
        }

        private JsonTableRow ToJsonTableRow(TableRow tableRow)
        {
            return new Mapper.TableRowToJsonTableRowMapper().Map(tableRow);
        }

        private JsonTable ToJsonTable(Table table)
        {
            return new Mapper.TableToJsonTableMapper().Map(table);
        }

        private JsonStep ToJsonStep(Step step)
        {
            return new Mapper.StepToJsonStepMapper().Map(step);
        }

        private JsonExample ToJsonExample(Example example)
        {
            return new Mapper.ExampleToJsonExampleMapper().Map(example);
        }

        private JsonScenarioOutline ToJsonScenarioOutline(ScenarioOutline scenarioOutline)
        {
            return new Mapper.ScenarioOutlineToJsonScenarioOutlineMapper().Map(scenarioOutline);
        }

        private JsonScenario ToJsonScenario(Scenario scenario)
        {
            return new Mapper.ScenarioToJsonScenarioMapper().Map(scenario);
        }

        private JsonFeature ToJsonFeature(Feature feature)
        {
            return new Mapper.FeatureToJsonFeatureMapper().Map(feature);
        }
    }
}
