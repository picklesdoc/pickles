//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Block_Tests.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests.Blocks_Tests
{
    [TestFixture]
    public class Block_Tests
    {
        [Test]
        public void ToString_Returns_Blocks_Markdown_Content()
        {
            var stylist = new MockStylist();
            Block mockBlock = new MockBlock(stylist);

            (mockBlock as MockBlock).Add("Hello, World");

            Assert.AreEqual("Hello, World\r\n", mockBlock.ToString());
        }

        [Test]
        public void Lines_Returns_A_Collection_Of_All_Block_Lines()
        {
            var stylist = new MockStylist();
            Block mockBlock = new MockBlock(stylist);

            (mockBlock as MockBlock).Add("Hello, World");

            Assert.AreEqual(1, mockBlock.Lines.Count);
        }
    }
}
