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
using PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks;
using NFluent;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class TitleBlock_Tests
    {
        [Test]
        public void A_New_TitleBlock_Has_Title_Line()
        {
            var expectedString = "Mocked Title Style: Features";
            var mockStyle = new MockStylist();

            var titleBlock = new TitleBlock(mockStyle);
            string actualString = titleBlock.ToString();

            Check.That(actualString).Contains(expectedString);
        }

        [Test]
        public void A_New_TitleBlock_Has_Generation_Line()
        {
            var expectedString = "Generated on: ";
            var mockStyle = new MockStylist();

            var titleBlock = new TitleBlock(mockStyle);
            string actualString = titleBlock.ToString();

            Check.That(actualString).Contains(expectedString);
        }
    }
}
