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
        public JsonMapper()
        {
        }

        public JsonFeature Map(Feature feature)
        {
            return this.ToJsonFeature(feature);
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
