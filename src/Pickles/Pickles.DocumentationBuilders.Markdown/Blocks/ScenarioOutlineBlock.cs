//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScenarioOutlineBlock.cs" company="PicklesDoc">
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
    class ScenarioOutlineBlock : Block
    {
        protected readonly ScenarioOutline scenarioOutline;

        public ScenarioOutlineBlock(ScenarioOutline scenarioOutline, Stylist style) : base(style)
        {
            this.scenarioOutline = scenarioOutline;

            this.lines = RenderedBlock();
        }

        private Lines RenderedBlock()
        {
            var lines = new Lines
            {
                AvailableTags(),

                Heading(),

                AvailableSteps(),

                Examples()
            };

            return lines;
        }

        private Lines AvailableTags()
        {
            var lines = new Lines();

            if (scenarioOutline.Tags.Count > 0)
            {
                var tagline = string.Empty;

                foreach (var tag in scenarioOutline.Tags)
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
            var lines = new Lines
            {
                style.AsScenarioOutlineHeading(scenarioOutline.Name)
            };

            return lines;
        }

        private Lines AvailableSteps()
        {
            var lines = new Lines();

            if (scenarioOutline.Steps.Count > 0)
            {
                foreach (var step in scenarioOutline.Steps)
                {
                    lines.Add(style.AsStepLine(string.Empty));
                    lines.Add(Step(step));
                }
            }

            return lines;
        }

        private Lines Examples()
        {
            var lines = new Lines();

            if (scenarioOutline.Examples != null)
            {
                foreach (var example in scenarioOutline.Examples)
                {
                    lines.Add(style.AsStepLine(string.Empty));

                    lines.Add(style.AsExampleHeading(example.Name));

                    lines.Add(style.AsStepLine(string.Empty));

                    var withResults = true;

                    if (scenarioOutline.Result == TestResult.NotProvided)
                    {
                        withResults = false;
                    }

                    var tableBlock = new TableBlock(example.TableArgument, style, withResults);

                    lines.Add(tableBlock.Lines);
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
