//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioOutlineToJsonScenarioOutlineMapperTests.cs" company="PicklesDoc">
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
    public class ScenarioOutlineToJsonScenarioOutlineMapperTests
    {
        [Test]
        public void Map_Null_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(null);

            Check.That(actual).IsNull();
        }

        private static ScenarioOutlineToJsonScenarioOutlineMapper CreateMapper()
        {
            return new ScenarioOutlineToJsonScenarioOutlineMapper();
        }

        [Test]
        public void Map_SomeScenarioOutline_ReturnsSomeJsonScenarioOutline()
        {
            var scenarioOutline = new ScenarioOutline();

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual).IsNotNull();
        }

        [Test]
        public void Map_NoExamples_ReturnsEmptyListOfExamples()
        {
            var scenarioOutline = new ScenarioOutline { Examples = null };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Examples.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_NoSteps_ReturnsEmtpyListOfSteps()
        {
            var scenarioOutline = new ScenarioOutline { Steps = null };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Steps.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_NoTags_ReturnsEmptyListOfTags()
        {
            var scenarioOutline = new ScenarioOutline { Tags = null };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Tags.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_WithName_ReturnsName()
        {
            var scenarioOutline = new ScenarioOutline { Name = "Some name" };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Name).IsEqualTo("Some name");
        }

        [Test]
        public void Map_WithDescription_ReturnsDescription()
        {
            var scenarioOutline = new ScenarioOutline { Description = "Some description" };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Description).IsEqualTo("Some description");
        }

        [Test]
        public void Map_WithResult_ReturnsResult()
        {
            var scenarioOutline = new ScenarioOutline { Result = TestResult.Passed };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Result.WasExecuted).IsTrue();
            Check.That(actual.Result.WasSuccessful).IsTrue();
        }

        [Test]
        public void Map_Always_ReturnsNullFeature()
        {
            var scenarioOutline = new ScenarioOutline
            {
                Feature = new Feature()
            };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Feature).IsNull();
        }

        [Test]
        public void Map_WithSteps_ReturnsSteps()
        {
            var scenarioOutline = new ScenarioOutline
            {
                Steps = new List<Step>
                {
                    new Step { Name = "I perform a step" }
                }
            };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Steps[0].Name).IsEqualTo("I perform a step");
        }

        [Test]
        public void Map_WithTags_ReturnsTags()
        {
            var scenarioOutline = new ScenarioOutline
            {
                Tags = new List<string>()
                {
                    "tag1",
                    "tag2"
                }
            };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Tags).ContainsExactly("tag1", "tag2");
        }

        [Test]
        public void Map_Example_ReturnsExample()
        {
            var scenarioOutline = new ScenarioOutline
            {
                Examples = new List<Example>
                {
                    new Example { Name = "Name of the example" }
                }
            };

            var mapper = CreateMapper();

            JsonScenarioOutline actual = mapper.Map(scenarioOutline);

            Check.That(actual.Examples[0].Name).IsEqualTo("Name of the example");
        }
    }
}