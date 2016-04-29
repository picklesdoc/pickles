//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExampleToJsonExampleMapperTests.cs" company="PicklesDoc">
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

using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.DocumentationBuilders.JSON.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class ExampleToJsonExampleMapperTests
    {
        [Test]
        public void Map_Null_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonExample actual = mapper.Map(null);

            Check.That(actual).IsNull();
        }

        private static ExampleToJsonExampleMapper CreateMapper()
        {
            return new ExampleToJsonExampleMapper();
        }

        [Test]
        public void Map_AnExample_ReturnsAJsonExample()
        {
            var example = new Example();

            var mapper = CreateMapper();

            var actual = mapper.Map(example);

            Check.That(actual).IsNotNull();
        }

        [Test]
        public void Map_AnExampleWithName_ReturnsAJsonExampleWithName()
        {
            var example = new Example { Name = "Some name"};

            var mapper = CreateMapper();

            var actual = mapper.Map(example);

            Check.That(actual.Name).IsEqualTo("Some name");
        }

        [Test]
        public void Map_AnExampleWithDescription_ReturnsAJsonExampleWithName()
        {
            var example = new Example { Description = "Some description" };

            var mapper = CreateMapper();

            var actual = mapper.Map(example);

            Check.That(actual.Description).IsEqualTo("Some description");
        }

        [Test]
        public void Map_AnExampleWithTableArgument_ReturnsAJsonExampleWithTableArgument()
        {
            var example = new Example
            {
                TableArgument = new Table { HeaderRow = new TableRow("Placeholder 1", "Placeholder 2") }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(example);

            Check.That(actual.TableArgument.HeaderRow).ContainsExactly("Placeholder 1", "Placeholder 2");
        }
    }
}