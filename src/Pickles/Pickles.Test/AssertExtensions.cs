//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AssertExtensions.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.Test
{
    public static class AssertExtensions
    {
        public static void HasAttribute(this ICheck<XElement> check, string name, string value)
        {
            var actual = ExtensibilityHelper.ExtractChecker(check).Value;

            XAttribute xmlAttribute = actual.Attributes().FirstOrDefault(attribute => attribute.Name.LocalName == name);
            Check.That(xmlAttribute).IsNotNull();
            // ReSharper disable once PossibleNullReferenceException
            Check.That(xmlAttribute.Value).IsEqualTo(value);
        }

        public static void HasElement(this ICheck<XElement> check, string name)
        {
            var actual = ExtensibilityHelper.ExtractChecker(check).Value;

            Check.That(actual.HasElement(name)).IsTrue();
        }

        public static void IsInNamespace(this ICheck<XElement> check, string nameOfNamespace)
        {
            var actual = ExtensibilityHelper.ExtractChecker(check).Value;

            Check.That(actual.Name.NamespaceName).IsEqualTo(nameOfNamespace);
        }

        public static void IsNamed(this ICheck<XElement> check, string name)
        {
            var actual = ExtensibilityHelper.ExtractChecker(check).Value;

            Check.That(actual.Name.LocalName).IsEqualTo(name);
        }

        public static void IsDeeplyEqualTo(this ICheck<XElement> check, XElement actual)
        {
            var element = ExtensibilityHelper.ExtractChecker(check).Value;

            if (!XNode.DeepEquals(element, actual))
            {
                var fluentMessage = FluentMessage.BuildMessage("The {0} is not equal to the given one (using deep comparison)").For(new EntityNamingLogic("XML element")).On(element.ToString()).And.WithGivenValue(actual.ToString());

                throw new FluentCheckException(fluentMessage.ToString());
            }
        }

        private static bool HasElement(this XElement element, string name)
        {
            return element.Elements().Any(e => e.Name.LocalName == name);
        }
    }
}
