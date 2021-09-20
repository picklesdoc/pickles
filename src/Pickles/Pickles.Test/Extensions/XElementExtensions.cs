//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XElementExtensions.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;
using NFluent;
using NFluent.Extensibility;
using NFluent.Kernel;
using NFluent.Messages;
using NUnit.Framework;

namespace PicklesDoc.Pickles.Test.Extensions
{
    public static class XElementExtensions
    {
        private static bool HasAttribute(this XElement element, string name, string value)
        {
            return element.Attribute(name) != null && element.Attribute(name).Value == value;
        }

        private static bool RecursiveSearch(this XElement element, Predicate<XElement> searchCriteria)
        {
            var meetsCriteria = searchCriteria(element);
            return meetsCriteria || element.Elements().Any(child => child.RecursiveSearch(searchCriteria));
        }

        public static ICheckLink<ICheck<XElement>> ContainsGherkinTable(this ICheck<XElement> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(
                () =>
                {
                    if (!checker.Value.RecursiveSearch(element => element.HasAttribute("class", "table_container")))
                    {
                        var errorMessage = FluentMessage.BuildMessage("The {0} does not contain a gherkin table (marked by the presence of a class attribute with value 'table_container')").For(new EntityNamingLogic("XML element")).On(checker.Value).ToString();
                        //Somehow NFluent does not recognise FluentCheckException on netcoreapp 3.1
                        throw new AssertionException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage("The {0} contains a gherkin table (marked by the presence of a class attribute with value 'table_container'), whereas it must not.").For(new EntityNamingLogic("XML element")).On(checker.Value).ToString());
        }

        public static ICheckLink<ICheck<XElement>> ContainsGherkinScenario(this ICheck<XElement> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!checker.Value.RecursiveSearch(element => element.HasAttribute("class", "scenario")))
                    {
                        var errorMessage = FluentMessage.BuildMessage("The {0} does not contain a gherkin scenario (marked by the presence of a class attribute with value 'scenario')").For(new EntityNamingLogic("XML element")).On(checker.Value).ToString();
                        throw new FluentCheckException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage("The {0} contains a gherkin scenario (marked by the presence of a class attribute with value 'scenario'), whereas it must not.").For(new EntityNamingLogic("XML element")).On(checker.Value).ToString());
        }
    }
}
