//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NamespaceExtensionsTests.cs" company="PicklesDoc">
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
using System.Xml.Linq;
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Html.Extensions;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class NamespaceExtensionsTests
    {
        private static readonly XNamespace NewNamespace = XNamespace.Get("http://myNewNamespace/");

        [Test]
        public void MoveNamespace()
        {
            var tree1 = new XElement(
                "Data",
                new XElement(
                    "Child",
                    "content",
                    new XAttribute("MyAttr", "content")));

            tree1.MoveToNamespace(NewNamespace);

            foreach (XElement node in tree1.DescendantsAndSelf())
            {
                Check.That(node).IsInNamespace(NewNamespace.NamespaceName);
            }
        }
    }
}
