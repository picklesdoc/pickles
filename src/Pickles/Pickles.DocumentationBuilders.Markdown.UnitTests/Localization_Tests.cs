//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Localization_Tests.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.DocumentationBuilders.Markdown;

namespace Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    class Localization_Tests
    {
        [Test]
        public void Localization_Has_String_For_Title()
        {
            var title = Localization.Title;

            Assert.IsNotNull(title);
            Assert.IsNotEmpty(title);
            Assert.AreEqual("Features", title);
        }
    }
}
