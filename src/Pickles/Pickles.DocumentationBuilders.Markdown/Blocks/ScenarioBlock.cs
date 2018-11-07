//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioBlock.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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

using PicklesDoc.Pickles.ObjectModel;
using System;
using System.Collections.Generic;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class ScenarioBlock
    {
        readonly Scenario scenario;
        readonly Stylist style;

        public ScenarioBlock(Scenario scenario, Stylist style)
        {
            this.scenario = scenario;
            this.style = style;
        }

        public new string ToString()
        {
            return RenderedBlock();
        }

        private string RenderedBlock()
        {
            var lines = new List<string>();

            lines = AddTagsIfAvailable(lines);

            lines = AddHeading(lines);

            lines = AddStepsIfAvailable(lines);

            return LineCollectionToString(lines);
        }

        private List<string> AddTagsIfAvailable(List<string> lines)
        {
            if (scenario.Tags.Count > 0)
            {
                var tagline = String.Empty;

                foreach (var tag in scenario.Tags)
                {
                    tagline = string.Concat(tagline, style.AsTag(tag), " ");
                }

                lines.Add(tagline.TrimEnd());
                lines.Add(string.Empty);
            }
            return lines;
        }

        private List<string> AddHeading(List<string> lines)
        {
            lines.Add(style.AsScenarioHeading(scenario.Name));

            return lines;
        }

        private List<string> AddStepsIfAvailable(List<string> lines)
        {
            if (scenario.Steps.Count > 0)
            {
                foreach (var step in scenario.Steps)
                {
                    lines.Add(style.AsStepLine(string.Empty));
                    lines = AddStep(lines, step);
                }
            }
            return lines;
        }

        private List<string> AddStep(List<string> lines, Step step)
        {
            var stepBlock = new StepBlock(step, style);

            lines.Add(stepBlock.ToString());

            return lines;
        }

        private string LineCollectionToString(List<string> lines)
        {
            string result = string.Empty;

            foreach (var line in lines)
            {
                result = string.Concat(result, line, Environment.NewLine);
            }

            return result;
        }
    }
}
