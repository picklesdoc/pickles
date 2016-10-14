//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DescriptionProcessor.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles
{
    public class DescriptionProcessor
    {
        public string Process(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            var splitLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (splitLines.Length == 1)
            {
                splitLines[0] = splitLines[0].TrimStart();
            }
            else
            {
                while (ThereAreNonEmptyLines(splitLines) && AllLinesAreEitherEmptyOrStartWithASpaceOrTab(splitLines))
                {
                    for (int i = 0; i < splitLines.Length; i++)
                    {
                        RemoveEmptySpaceFromNonEmptyLines(splitLines, i);
                    }
                }
            }

            for (int index = 0; index < splitLines.Length; index++)
            {
                splitLines[index] = splitLines[index].Replace("\\#", "#");
            }

            var result = string.Join(Environment.NewLine, splitLines);

            return result;
        }

        public void Process(Feature feature)
        {
            feature.Description = Process(feature.Description);

            foreach (var featureElement in feature.FeatureElements)
            {
                featureElement.Description = Process(featureElement.Description);
            }

            if (feature.Background != null)
            {
                feature.Background.Description = Process(feature.Background.Description);
            }
        }

        private static void RemoveEmptySpaceFromNonEmptyLines(string[] splitLines, int index)
        {
            if (splitLines[index].Length != 0)
            {
                splitLines[index] = splitLines[index].Substring(1);
            }
        }

        private static bool AllLinesAreEitherEmptyOrStartWithASpaceOrTab(string[] splitLines)
        {
            return splitLines.All(s => s.Length == 0 || s[0] == ' ') || splitLines.All(s => s.Length == 0 || s[0] == '\t');
        }

        private static bool ThereAreNonEmptyLines(string[] splitLines)
        {
            return splitLines.Any(s => s.Length > 0);
        }
    }
}