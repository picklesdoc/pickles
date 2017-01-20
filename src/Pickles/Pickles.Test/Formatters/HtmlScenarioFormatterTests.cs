//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlScenarioFormatterTests.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.Formatters
{
    [TestFixture]
    public class HtmlScenarioFormatterTests : BaseFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            this.formatter = new HtmlScenarioFormatter(
                Container.Resolve<HtmlStepFormatter>(),
                Container.Resolve<HtmlDescriptionFormatter>(),
                Container.Resolve<HtmlImageResultFormatter>());
        }

        #endregion

        private HtmlScenarioFormatter formatter;

        private Scenario BuildMinimalScenario()
        {
            return new Scenario
            {
                Description = "My Scenario Description",
                Steps = new List<Step>
                {
                    new Step
                    {
                        NativeKeyword = "Given",
                        Name = "My Step Name",
                    }
                }
            };
        }

        [Test]
        public void Li_Element_Must_Not_Have_Id_Attribute()
        {
            Scenario scenario = this.BuildMinimalScenario();

            XElement li = this.formatter.Format(scenario, 1);

            XAttribute idAttribute = li.Attribute("id");

            Check.That(idAttribute).IsNull();
        }
    }
}
