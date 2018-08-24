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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.VsTest
{
    public class VsTestScenarioOutlineExampleMatcher : IScenarioOutlineExampleMatcher
    {
        private static readonly Regex VariantWithExampleGroupRegex = new Regex(@"(?:[\S]+)_ExampleSet([\d]+)_Variant([\d]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex VariantRegex = new Regex(@"(?:[\S]+)_Variant([\d]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ExampleGroupRegex = new Regex(@"(?:[^_\s]+)_([^_\s]+)_([\S]*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool IsMatch(ScenarioOutline scenarioOutline, string[] exampleValues, object scenarioElement)
        {
            var element = (XElement)scenarioElement;
            var elementName = element.Name().ToUpperInvariant();

            bool isMatch;

            if (IsVariantWithExampleGroupMatch(scenarioOutline, exampleValues, elementName, out isMatch))
            {
                return isMatch;
            }

            if (IsVariantWithoutExampleGroupMatch(scenarioOutline, exampleValues, elementName, out isMatch))
            {
                return isMatch;
            }

            if (IsExampleGroupMatch(scenarioOutline, exampleValues, elementName, out isMatch))
            {
                return isMatch;
            }

            var matchValue = Normalize(exampleValues[0]);

            isMatch = element.Name().ToUpperInvariant()
                .EndsWith(matchValue);

            return isMatch;
        }

        private bool IsVariantWithExampleGroupMatch(ScenarioOutline scenarioOutline, string[] exampleValues, string elementName, out bool isMatch)
        {
            const int ExampleGroup = 1;
            const int VariantNumberGroup = 2;

            var match = VariantWithExampleGroupRegex.Match(elementName);

            if (match.Success)
            {
                int exampleGroupNumber = int.Parse(match.Groups[ExampleGroup].Value);

                var exampleSet = scenarioOutline.Examples?[exampleGroupNumber];
                if (exampleSet != null)
                {
                    int variantNumber;
                    if (int.TryParse(match.Groups[VariantNumberGroup].Value, out variantNumber))
                    {
                        var examples = exampleSet.TableArgument.DataRows;
                        var example = examples[variantNumber];

                        isMatch = example.Cells.SequenceEqual(exampleValues);

                        return true;
                    }
                }
            }

            isMatch = false;

            return false;
        }

        private bool IsVariantWithoutExampleGroupMatch(ScenarioOutline scenarioOutline, string[] exampleValues, string elementName, out bool isMatch)
        {
            const int VariantNumberGroup = 1;

            var match = VariantRegex.Match(elementName);
            if (match.Success)
            {
                int variantNumber;
                if (int.TryParse(match.Groups[VariantNumberGroup].Value, out variantNumber))
                {
                    List<TableRow> examples = null;
                    if (scenarioOutline.Examples?.Count == 1)
                    {
                        examples = scenarioOutline.Examples[0].TableArgument.DataRows;
                    }
                    else if (scenarioOutline.Examples?.Any(x => x.Name == string.Empty) == true)
                    {
                        examples = scenarioOutline.Examples?.First(x => x.Name == string.Empty).TableArgument.DataRows;
                    }
                    else
                    {
                        isMatch = false;
                        return false;
                    }

                    var example = examples[variantNumber];

                    isMatch = example.Cells.SequenceEqual(exampleValues);

                    return true;
                }
            }

            isMatch = false;
            return false;
        }

        private bool IsExampleGroupMatch(ScenarioOutline scenarioOutline, string[] exampleValues, string elementName, out bool isMatch)
        {
            const int ExampleGroupNameGroup = 1;
            const int MatchValueGroup = 2;
            const int VariantNumberGroup = 1;

            var match = ExampleGroupRegex.Match(elementName);
            if (match.Success)
            {
                string exampleGroupName = match.Groups[ExampleGroupNameGroup].Value;

                var exampleSet = scenarioOutline.Examples?.FirstOrDefault(x => Normalize(x.Name) == exampleGroupName);

                var variantMatch = VariantRegex.Match(elementName);
                if (variantMatch.Success)
                {
                    int variantNumber;
                    if (int.TryParse(variantMatch.Groups[VariantNumberGroup].Value, out variantNumber))
                    {
                        isMatch = exampleSet.TableArgument.DataRows[variantNumber].Cells.SequenceEqual(exampleValues);
                        return true;
                    }

                    isMatch = false;
                    return false;
                }

                if (exampleSet != null)
                {
                    var matchValue = match.Groups[MatchValueGroup].Value;
                    var examples = exampleSet.TableArgument.DataRows;

                    var example = examples.Single(x => Normalize(x.Cells[0]) == matchValue);
                    isMatch = example.Cells.SequenceEqual(exampleValues);

                    return true;
                }
            }

            isMatch = false;
            return false;
        }

        private string Normalize(string value)
        {
            return value?
                .Replace(" ", string.Empty)
                .Replace(":", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace(".", "_")
                .Replace("-", "_")
                .Replace(",", string.Empty)
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
        }
    }
}