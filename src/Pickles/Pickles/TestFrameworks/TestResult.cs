#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public struct TestResult
    {
        public bool WasExecuted;
        public bool WasSuccessful;
        public bool WasNotFound;

        public static TestResult Passed()
        {
          return new TestResult { WasExecuted = true, WasSuccessful = true };
        }

        public static TestResult Failed()
        {
          return new TestResult { WasExecuted = true, WasSuccessful = false };
        }

        public static TestResult Inconclusive()
        {
          return new TestResult { WasExecuted = false, WasSuccessful = false };
        }

        public static TestResult NotFound()
        {
            return new TestResult {WasExecuted = false, WasSuccessful = false, WasNotFound = true};
        }
    }

  public static class TestResultExtensions
  {
    public static TestResult Merge(this IEnumerable<TestResult> testResults)
    {
      if (testResults == null)
      {
        throw new ArgumentNullException("testResults");
      }

      TestResult[] items = testResults.ToArray();

      if (!items.Any())
      {
        return new TestResult();
      }

      if (items.Count() == 1)
      {
        return items.Single();
      }

      return new TestResult
        {
          WasExecuted = items.All(i => i.WasExecuted),
          WasSuccessful = items.All(i => i.WasSuccessful),
        };
    }
  }
}