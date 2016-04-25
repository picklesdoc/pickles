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
    public class TestResultToJsonTestResultMapperTests
    {
        [Test]
        public void Map_Inconclusive_ReturnsExecutedFalseSuccessfulFalse()
        {
            var testResult = TestResult.Inconclusive;

            var mapper = CreateMapper();

            JsonTestResult actual = mapper.Map(testResult);

            Check.That(actual.WasExecuted).IsFalse();
            Check.That(actual.WasSuccessful).IsFalse();
        }

        private static TestResultToJsonTestResultMapper CreateMapper()
        {
            return new TestResultToJsonTestResultMapper();
        }

        [Test]
        public void Map_Failed_ReturnsExecutedTrueSuccessfulFalse()
        {
            var testResult = TestResult.Failed;

            var mapper = CreateMapper();

            var actual = mapper.Map(testResult);

            Check.That(actual.WasExecuted).IsTrue();
            Check.That(actual.WasSuccessful).IsFalse();
        }

        [Test]
        public void Map_Passed_ReturnsExecutedTrueSuccessfulTrue()
        {
            var testResult = TestResult.Passed;

            var mapper = CreateMapper();

            var actual = mapper.Map(testResult);

            Check.That(actual.WasExecuted).IsTrue();
            Check.That(actual.WasSuccessful).IsTrue();
        }
    }
}