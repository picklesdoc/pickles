//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="VsTestScenarioOutlineExampleMatcher.cs" company="PicklesDoc">
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

using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.VsTest
{
    public class VsTestScenarioOutlineExampleMatcher : IScenarioOutlineExampleMatcher
    {
        private static readonly Regex VariantRegex = new Regex(@"(.*)_Variant([\d*])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const int VariantNumberGroup = 2;

        public bool IsMatch(ScenarioOutline scenarioOutline, string[] exampleValues, object scenarioElement)
        {
            var element = (XElement)scenarioElement;

            var matchValue = exampleValues[0]
                .Replace(" ", string.Empty)
                .Replace(":", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace(".", "_")
                .Replace("!", string.Empty)
                .Replace("&", string.Empty)
                .ToUpperInvariant()
                .Replace("Ä", "A")
                .Replace("Ö", "O")
                .Replace("Ü", "U")
                .Replace("ß", "B")
                .Replace("æ", "ae")
                .Replace('ø', 'o')
                .Replace('å', 'a')
                .Replace("Æ", "AE")
                .Replace('Ø', 'O')
                .Replace('Å', 'A');

            var variantMatch = VariantRegex.Match(element.Name().ToUpperInvariant());
            if (variantMatch.Success)
            {
                int variantNumber;
                if (int.TryParse(variantMatch.Groups[VariantNumberGroup].Value, out variantNumber))
                {
                    if (scenarioOutline.Examples?.Count > 0)
                    {
                        var allExamples = scenarioOutline.Examples.SelectMany(x => x.TableArgument.DataRows);
                        var example = allExamples.ElementAt(variantNumber);

                        return example.Cells.SequenceEqual(exampleValues);
                    }
                }
            }

            var isMatch = element.Name().ToUpperInvariant()
                .EndsWith(matchValue);

            return isMatch;
        }
    }
}
