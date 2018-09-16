//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestResult.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.ObjectModel
{
    public enum TestResult
    {
        Inconclusive,
        Failed,
        Passed,
        NotProvided
    }

    public static class TestResultExtensions
    {
        public static TestResult Merge(this IEnumerable<TestResult> testResults, bool passedTrumpsInconclusive = false)
        {
            if (testResults == null)
            {
                throw new ArgumentNullException(nameof(testResults));
            }

            TestResult[] items = testResults.ToArray();

            if (!items.Any())
            {
                return TestResult.Inconclusive;
            }

            if (items.Length == 1)
            {
                return items.Single();
            }

            if (items.Any(i => i == TestResult.Failed))
            {
                return TestResult.Failed;
            }

            if (passedTrumpsInconclusive)
            {
                if (items.Any(r => r == TestResult.Passed))
                {
                    return TestResult.Passed;
                }

                return TestResult.Inconclusive;
            }
            else
            {
                if (items.Any(i => i == TestResult.Inconclusive))
                {
                    return TestResult.Inconclusive;
                }

                return TestResult.Passed;
            }
        }
    }
}
