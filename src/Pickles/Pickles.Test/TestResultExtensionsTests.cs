//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestResultExtensionsTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class TestResultExtensionsTests
    {
        [Test]
        public void Merge_NullReference_ThrowsArgumentNullException()
        {
            Check.ThatCode(() => TestResultExtensions.Merge(null)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Merge_EmptySequence_ResultsInWasExecutedFalseAndWasSuccessfulFalse()
        {
            var testResults = new TestResult[0];

            TestResult actual = testResults.Merge();

            Check.That(actual).Equals(TestResult.Inconclusive);
        }

        [Test]
        public void Merge_SingleItem_ReturnsThatItem()
        {
            var testResults = new[]
            {
                TestResult.Passed
            };

            TestResult actual = testResults.Merge();

            Check.That(actual).Equals(TestResult.Passed);
        }

        [Test]
        public void Merge_MultiplePassedResults_ShouldReturnPassed()
        {
            var testResults = new[] { TestResult.Passed, TestResult.Passed };

            TestResult actual = testResults.Merge();

            Check.That(actual).Equals(TestResult.Passed);
        }

        [Test]
        public void Merge_MultiplePassedOneInconclusiveResults_ShouldReturnInconclusive()
        {
            var testResults = new[] { TestResult.Passed, TestResult.Passed, TestResult.Inconclusive };

            TestResult actual = testResults.Merge();

            Check.That(actual).Equals(TestResult.Inconclusive);
        }

        [Test]
        public void Merge_PassedInconclusiveAndFailedResults_ShouldReturnFailed()
        {
            var testResults = new[] { TestResult.Passed, TestResult.Inconclusive, TestResult.Failed };

            TestResult actual = testResults.Merge();

            Check.That(actual).Equals(TestResult.Failed);
        }
    }
}
