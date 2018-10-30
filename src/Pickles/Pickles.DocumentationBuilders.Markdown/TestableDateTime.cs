//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestableDateTime.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    class TestableDateTime
    {
        #region Singleton
        private static Lazy<TestableDateTime> _lazyInstance = new Lazy<TestableDateTime>(() => new TestableDateTime());
        private TestableDateTime()
        {
        }
        public static TestableDateTime Instance
        {
            get
            {
                return _lazyInstance.Value;
            }
        }
        #endregion

        private Func<DateTime> _defaultCurrentFunction = () => DateTime.Now;

        public DateTime Now
        {
            get{
                if (DisposableTestDateTime.Current == null)
                {
                    return _defaultCurrentFunction.Invoke();
                }
                else
                {
                    return DisposableTestDateTime.Current.TestDateTimeNow;
                }
            }
        }
    }
}
