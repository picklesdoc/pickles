//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureBlock.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class FeatureBlock : Block
    {
        readonly Feature feature;

        public FeatureBlock(Feature feature, Stylist style) : base(style)
        {
            this.feature = feature;
            this.lines = RenderedBlock();
        }

        private Lines RenderedBlock()
        {
            var lines = new Lines
            {
                AvailableFeatureTags(),

                Heading(),

                AvailableDescription(),

                AvailableBackground(),

                AvailableFeatureElements()
            };

            return lines;
        }

        private Lines AvailableFeatureTags()
        {
            var lines = new Lines();

            if (feature.Tags.Count > 0)
            {
                var tagline = string.Empty;

                foreach (var tag in feature.Tags)
                {
                    tagline = string.Concat(tagline, style.AsTag(tag), " ");
                }

                lines.Add(tagline.TrimEnd());
                lines.Add(string.Empty);
            }
            return lines;
        }

        private Lines Heading()
        {
            var lines = new Lines
            {
                style.AsFeatureHeading(feature.Name),
                string.Empty
            };

            return lines;
        }

        private Lines AvailableDescription()
        {
            var lines = new Lines();

            if (feature.Description != null)
            {
                foreach (var descriptionLine in feature.Description.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    lines.Add(descriptionLine);
                    lines.Add(string.Empty);
                }
            }

            return lines;
        }

        private Lines AvailableBackground()
        {
            if (feature.Background != null)
            {
                var backgroundBlock = new BackgroundBlock(feature.Background, style);

                return backgroundBlock.Lines;
            }

            return new Lines();
        }

        private Lines AvailableFeatureElements()
        {
            var lines = new Lines();

            if (feature.FeatureElements.Count > 0)
            {
                foreach (var element in feature.FeatureElements)
                {
                    if (element.GetType() == typeof(Scenario))
                    {
                        lines.Add(Scenario(element as Scenario));

                        lines.Add(style.AsStepLine(string.Empty));
                    }
                    else if (element.GetType() == typeof(ScenarioOutline))
                    {
                        lines.Add(ScenarioOutline(element as ScenarioOutline));

                        lines.Add(style.AsStepLine(string.Empty));
                    }
                }
            }

            return lines;
        }

        private Lines Scenario(Scenario scenario)
        {
            var scenarioBlock = new ScenarioBlock(scenario, style);

            return scenarioBlock.Lines;
        }

        private Lines ScenarioOutline(ScenarioOutline scenarioOutline)
        {
            var scenarioOutlineBlock = new ScenarioOutlineBlock(scenarioOutline, style);

            return scenarioOutlineBlock.Lines;       
        }
    }
}
