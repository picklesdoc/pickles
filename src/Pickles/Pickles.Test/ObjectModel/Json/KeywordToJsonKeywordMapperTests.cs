//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestResultToJsonTestResultMapperTests.cs" company="PicklesDoc">
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
    public class KeywordToJsonKeywordMapperTests
    {
        [Test]
        public void Map_Given_ReturnsGiven()
        {
            var keyword = Keyword.Given;

            var mapper = CreateMapper();

            JsonKeyword actual = mapper.Map(keyword);

            Check.That(actual).IsEqualTo(JsonKeyword.Given);
        }

        private static KeywordToJsonKeywordMapper CreateMapper()
        {
            return new KeywordToJsonKeywordMapper();
        }

        [Test]
        public void Map_When_ReturnsWhen()
        {
            var keyword = Keyword.When;

            var mapper = CreateMapper();

            JsonKeyword actual = mapper.Map(keyword);

            Check.That(actual).IsEqualTo(JsonKeyword.When);
        }

        [Test]
        public void Map_Then_ReturnsThen()
        {
            var keyword = Keyword.Then;

            var mapper = CreateMapper();

            JsonKeyword actual = mapper.Map(keyword);

            Check.That(actual).IsEqualTo(JsonKeyword.Then);
        }

        [Test]
        public void Map_And_ReturnsAnd()
        {
            var keyword = Keyword.And;

            var mapper = CreateMapper();

            JsonKeyword actual = mapper.Map(keyword);

            Check.That(actual).IsEqualTo(JsonKeyword.And);
        }

        [Test]
        public void Map_But_ReturnsBut()
        {
            var keyword = Keyword.But;

            var mapper = CreateMapper();

            JsonKeyword actual = mapper.Map(keyword);

            Check.That(actual).IsEqualTo(JsonKeyword.But);
        }
    }
}