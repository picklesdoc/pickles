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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class ScenarioBlock : Block
    {
        protected readonly Scenario scenario;

        public ScenarioBlock(Scenario scenario, Stylist style) : base(style)
        {
            this.scenario = scenario;

            this.lines = RenderedBlock();
        }

        private Lines RenderedBlock()
        {
            var lines = new Lines
            {
                AvailableTags(),

                Heading(),

                AvailableSteps()
            };

            return lines;
        }

        private Lines AvailableTags()
        {
            var lines = new Lines();

            if (scenario.Tags.Count > 0)
            {
                var tagline = string.Empty;

                foreach (var tag in scenario.Tags)
                {
                    tagline = string.Concat(tagline, style.AsTag(tag), " ");
                }

                lines.Add(tagline.TrimEnd());
                lines.Add(string.Empty);
            }

            return lines;
        }

        protected virtual Lines Heading()
        {
            var lines = new Lines();

            if(scenario.Result != TestResult.NotProvided)
            {
                lines.Add(style.AsScenarioHeading(scenario.Name, scenario.Result));
            }
            else
            {
                lines.Add(style.AsScenarioHeading(scenario.Name));
            }

            return lines;
        }

        private Lines AvailableSteps()
        {
            var lines = new Lines();

            if (scenario.Steps.Count > 0)
            {
                foreach (var step in scenario.Steps)
                {
                    lines.Add(style.AsStepLine(string.Empty));
                    lines.Add(Step(step));
                }
            }

            return lines;
        }

        private Lines Step(Step step)
        {
            var stepBlock = new StepBlock(step, style);

            return stepBlock.Lines;
        }
    }
}
