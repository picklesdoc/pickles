//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestableDateTime_Tests.cs" company="PicklesDoc">
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

using NUnit.Framework;
using System;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    class TestableDateTime_Tests
    {
        [Test]
        public void DateTimeProvider_Without_Context_Returns_DateTime_Now()
        {
            var dateTimeDiff = (int)(TestableDateTime.Instance.Now - DateTime.Now).TotalMilliseconds;
            Assert.IsTrue(dateTimeDiff < 100);
        }

        [Test]
        public void DateTimeProvider_With_Context_Returns_DateTime_You_Specified()
        {
            var expectedDateTime = new DateTime(2018, 5, 20, 10, 59, 59, DateTimeKind.Local);
            using (var DateTimeContext = new DisposableTestDateTime(expectedDateTime))
            {
                var actualDateTime = TestableDateTime.Instance.Now;

                Assert.AreEqual(expectedDateTime, actualDateTime);
            }
        }
    }
}
