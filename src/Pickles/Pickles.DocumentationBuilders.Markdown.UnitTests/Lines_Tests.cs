//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Lines_Tests.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class Lines_Tests
    {
        [Test]
        public void An_Empty_Lines_Object_Has_No_Lines()
        {
            var lines = new Lines();

            Assert.AreEqual(0, lines.Count);
        }

        [Test]
        public void Adding_A_Line_Increases_Count()
        {
            var lines = new Lines();

            lines.Add("Hello, World");

            Assert.AreEqual(1, lines.Count);
        }

        [Test]
        public void Adding_A_Lines_Object_Together_Combines_Them()
        {
            var lines = new Lines();
            lines.Add("Hello, World");

            var moreLines = new Lines();
            moreLines.Add("Hello");
            moreLines.Add("World");

            lines.Add(moreLines);

            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual(2, moreLines.Count);
        }

        [Test]
        public void Converting_Lines_Object_To_String_Returns_All_Lines_With_NewLine_Seperator()
        {
            var lines = new Lines();
            lines.Add("Hello");
            lines.Add("World");

            Assert.AreEqual("Hello\r\nWorld\r\n", lines.ToString());
        }
    }
}
