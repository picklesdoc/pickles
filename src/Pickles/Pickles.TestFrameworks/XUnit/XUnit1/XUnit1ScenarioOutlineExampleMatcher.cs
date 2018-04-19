//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XUnit1ScenarioOutlineExampleMatcher.cs" company="PicklesDoc">
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

using System.Text.RegularExpressions;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.XUnit.XUnit1
{
    public class XUnit1ScenarioOutlineExampleMatcher : IScenarioOutlineExampleMatcher
    {
        private readonly XUnitExampleSignatureBuilder signatureBuilder = new XUnitExampleSignatureBuilder();

        public bool IsMatch(ScenarioOutline scenarioOutline, string[] exampleValues, object scenarioElement)
        {
            var build = this.signatureBuilder.Build(scenarioOutline, exampleValues);

            return IsMatchingTestCase((XElement)scenarioElement, build);
        }

        internal static bool IsMatchingTestCase(XElement x, Regex exampleSignature)
        {
            return exampleSignature.IsMatch(x.Attribute("name").Value);
        }
    }
}