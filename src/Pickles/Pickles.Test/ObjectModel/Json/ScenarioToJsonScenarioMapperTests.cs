//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioToJsonScenarioMapperTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.DocumentationBuilders.JSON.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class ScenarioToJsonScenarioMapperTests
    {
        [Test]
        public void Map_Null_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(null);

            Check.That(actual).IsNull();
        }

        private static ScenarioToJsonScenarioMapper CreateMapper()
        {
            return new ScenarioToJsonScenarioMapper();
        }

        [Test]
        public void Map_SomeScenario_ReturnsSomeJsonScenario()
        {
            var scenario = new Scenario();

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual).IsNotNull();
        }

        [Test]
        public void Map_NoSteps_ReturnsEmtpyListOfSteps()
        {
            var scenario = new Scenario { Steps = null };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Steps.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_NoTags_ReturnsEmptyListOfTags()
        {
            var scenario = new Scenario { Tags = null };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Tags.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_WithName_ReturnsName()
        {
            var scenario = new Scenario { Name = "Some name" };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Name).IsEqualTo("Some name");
        }

        [Test]
        public void Map_WithName_ReturnsSlug()
        {
            var scenario = new Scenario { Slug = "i-am-a-slug" };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Slug).IsEqualTo("i-am-a-slug");
        }

        [Test]
        public void Map_WithDescription_ReturnsDescription()
        {
            var scenario = new Scenario { Description = "Some description" };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Description).IsEqualTo("Some description");
        }

        [Test]
        public void Map_WithResult_ReturnsResult()
        {
            var scenario = new Scenario { Result = TestResult.Passed };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Result.WasExecuted).IsTrue();
            Check.That(actual.Result.WasSuccessful).IsTrue();
        }

        [Test]
        public void Map_Always_ReturnsNullFeature()
        {
            var scenario = new Scenario
            {
                Feature = new Feature()
            };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Feature).IsNull();
        }

        [Test]
        public void Map_WithSteps_ReturnsSteps()
        {
            var scenario = new Scenario
            {
                Steps = new List<Step>
                {
                    new Step { Name = "I perform a step" }
                }
            };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Steps[0].Name).IsEqualTo("I perform a step");
        }

        [Test]
        public void Map_WithTags_ReturnsTags()
        {
            var scenario = new Scenario
            {
                Tags = new List<string>()
                {
                    "tag1",
                    "tag2"
                }
            };

            var mapper = CreateMapper();

            JsonScenario actual = mapper.Map(scenario);

            Check.That(actual.Tags).ContainsExactly("tag1", "tag2");
        }
    }
}