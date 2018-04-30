//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingScenarioOutlines.cs" company="PicklesDoc">
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

using Autofac;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;
using PicklesDoc.Pickles.Test.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests
{
    [TestFixture]
    public class WhenFormattingScenarioOutlines : BaseFixture
    {
        [Test]
        public void ThenCanFormatCompleteScenarioOutlineCorrectly()
        {
            var table = new ExampleTable
            {
                HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4"),
                DataRows =
                    new List<TableRow>(new[]
                    {
                        new TableRow("1", "2", "3", "4"),
                        new TableRow("5", "6", "7", "8")
                    })
            };

            var example = new Example { Name = "Some examples", Description = "An example", TableArgument = table };
            var examples = new List<Example>();
            examples.Add(example);

            var scenarioOutline = new ScenarioOutline
            {
                Name = "Testing a scenario outline",
                Description = "We need to make sure that scenario outlines work properly",
                Examples = examples
            };

            var htmlScenarioOutlineFormatter = Container.Resolve<HtmlScenarioOutlineFormatter>();
            var output = htmlScenarioOutlineFormatter.Format(scenarioOutline, 0);

            Check.That(output).ContainsGherkinScenario();
            Check.That(output).ContainsGherkinTable();
        }

        [Test]
        public void ThenCanFormatScenarioOutlineWithMissingNameCorrectly()
        {
            var table = new ExampleTable
            {
                HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4"),
                DataRows =
                    new List<TableRow>(new[]
                    {
                        new TableRow("1", "2", "3", "4"),
                        new TableRow("5", "6", "7", "8")
                    })
            };

            var example = new Example { Name = "Some examples", Description = "An example", TableArgument = table };
            var examples = new List<Example>();
            examples.Add(example);

            var scenarioOutline = new ScenarioOutline
            {
                Description = "We need to make sure that scenario outlines work properly",
                Examples = examples
            };

            var htmlScenarioOutlineFormatter = Container.Resolve<HtmlScenarioOutlineFormatter>();
            var output = htmlScenarioOutlineFormatter.Format(scenarioOutline, 0);

            Check.That(output).ContainsGherkinScenario();
            Check.That(output).ContainsGherkinTable();
        }

        [Test]
        public void ThenCanFormatScenarioOutlineWithMissingDescriptionCorrectly()
        {
            var table = new ExampleTable
            {
                HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4"),
                DataRows =
                    new List<TableRow>(new[]
                    {
                        new TableRow("1", "2", "3", "4"),
                        new TableRow("5", "6", "7", "8")
                    })
            };

            var example = new Example { Name = "Some examples", Description = "An example", TableArgument = table };
            var examples = new List<Example>();
            examples.Add(example);

            var scenarioOutline = new ScenarioOutline
            {
                Name = "Testing a scenario outline",
                Examples = examples
            };

            var htmlScenarioOutlineFormatter = Container.Resolve<HtmlScenarioOutlineFormatter>();
            var output = htmlScenarioOutlineFormatter.Format(scenarioOutline, 0);

            Check.That(output).ContainsGherkinScenario();
            Check.That(output).ContainsGherkinTable();
        }

        [Test]
        public void ThenCanFormatScenarioOutlineWithMissingExampleCorrectly()
        {
            var scenarioOutline = new ScenarioOutline
            {
                Name = "Testing a scenario outline",
                Description = "We need to make sure that scenario outlines work properly",
                Examples = new List<Example>()
            };

            var htmlScenarioOutlineFormatter = Container.Resolve<HtmlScenarioOutlineFormatter>();
            var output = htmlScenarioOutlineFormatter.Format(scenarioOutline, 0);

            Check.That(output).ContainsGherkinScenario();
            Check.That(output).Not.ContainsGherkinTable();
        }

        [Test]
        public void ThenCanFormatScenarioOutlineWithMissingTableFromExampleCorrectly()
        {
            var example = new Example { Name = "Some examples", Description = "An example" };
            var examples = new List<Example>();
            examples.Add(example);

            var scenarioOutline = new ScenarioOutline
            {
                Name = "Testing a scenario outline",
                Description = "We need to make sure that scenario outlines work properly",
                Examples = examples
            };

            var htmlScenarioOutlineFormatter = Container.Resolve<HtmlScenarioOutlineFormatter>();
            var output = htmlScenarioOutlineFormatter.Format(scenarioOutline, 0);

            Check.That(output).ContainsGherkinScenario();
            Check.That(output).Not.ContainsGherkinTable();
        }
    }
}
