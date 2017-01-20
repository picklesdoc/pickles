//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingTables.cs" company="PicklesDoc">
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
using System.Xml.Linq;

using Autofac;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests
{
    [TestFixture]
    public class WhenFormattingTables : BaseFixture
    {
        [Test]
        public void ThenCanFormatNormalTableSuccessfully()
        {
            var table = new Table
            {
                HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4"),
                DataRows =
                    new List<TableRow>(new[]
                    {
                        new TableRow("1", "2", "3", "4"),
                        new TableRow("5", "6", "7", "8")
                    })
            };

            var htmlTableFormatter = Container.Resolve<HtmlTableFormatter>();

            var output = htmlTableFormatter.Format(table);

            Check.That(output).IsNotNull();
            Check.That(output).HasAttribute("class", "table_container");
            Check.That(output).HasElement("table");

            var tableElement = output.Element(XName.Get("table", HtmlNamespace.Xhtml.NamespaceName));
            Check.That(tableElement).HasElement("thead");
            Check.That(tableElement).HasElement("tbody");
        }

        [Test]
        public void ThenCanFormatNullTableSuccessfully()
        {
            var htmlTableFormatter = Container.Resolve<HtmlTableFormatter>();
            var output = htmlTableFormatter.Format(null);

            Check.That(output).IsNull();
        }
    }
}
