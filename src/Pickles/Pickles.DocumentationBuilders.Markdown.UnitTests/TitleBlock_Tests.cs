//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TitleBlock_Tests.cs" company="PicklesDoc">
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
    public class TitleBlock_Tests
    {
        [Test]
        public void A_New_TitleBlock_Has_Title_Line()
        {
            var mockStyle = new MockStylist();

            var titleBlock = new TitleBlock(mockStyle);

            Assert.IsTrue(titleBlock.Text.Contains("Mocked Title Style: Features"));
        }

        [Test]
        public void A_New_TitleBlock_Has_Generation_Line()
        {
            var expectedDateTime = new DateTime(2018, 10, 25, 18, 53, 00, DateTimeKind.Local);
            using (var DateTimeContext = new DisposableTestDateTime(expectedDateTime))
            {
                var mockStyle = new MockStylist();

                var titleBlock = new TitleBlock(mockStyle);

                Assert.IsTrue(titleBlock.Text.Contains("Generated on: 25 October 2018 at 18:53:00"));
            }
        }
    }
}
