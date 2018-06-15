//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Mapper.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.Linq;

using PicklesDoc.Pickles.Extensions;

using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.ObjectModel
{

    public class Mapper
    {
        private readonly IConfiguration configuration;

        private readonly ILanguageServices languageServices;

        public Mapper(IConfiguration configuration, ILanguageServices languageServices)
        {
            this.configuration = configuration;
            this.languageServices = languageServices;
        }

        public string MapToString(G.TableCell cell)
        {
            return cell?.Value;
        }

        public TableRow MapToTableRow(G.TableRow tableRow)
        {
            if (tableRow == null)
            {
                return null;
            }

            return new TableRow(tableRow.Cells.Select(this.MapToString));
        }

        public Table MapToTable(G.DataTable dataTable)
        {
            if (dataTable == null)
            {
                return null;
            }

            var tableRows = dataTable.Rows;
            return this.MapToTable(tableRows);
        }

        public Table MapToTable(IEnumerable<G.TableRow> tableRows)
        {
            return new Table
            {
                HeaderRow = this.MapToTableRow(tableRows.First()),
                DataRows = tableRows.Skip(1).Select(MapToTableRow).ToList()
            };
        }

        public TableRow MapToTableRowWithTestResult(G.TableRow tableRow)
        {
            if (tableRow == null)
            {
                return null;
            }

            return new TableRowWithTestResult(tableRow.Cells.Select(this.MapToString));
        }

        public Table MapToExampleTable(G.DataTable dataTable)
        {
            if (dataTable == null)
            {
                return null;
            }

            var tableRows = dataTable.Rows;
            return this.MapToExampleTable(tableRows);
        }

        public ExampleTable MapToExampleTable(IEnumerable<G.TableRow> tableRows)
        {
            return new ExampleTable
            {
                HeaderRow = this.MapToTableRow(tableRows.First()),
                DataRows = tableRows.Skip(1).Select(MapToTableRowWithTestResult).ToList()
            };
        }

        public string MapToString(G.DocString docString)
        {
            return docString?.Content;
        }

        public Step MapToStep(G.Step step)
        {
            if (step == null)
            {
                return null;
            }

            return new Step
            {
                Location = this.MapToLocation(step.Location),
                DocStringArgument = step.Argument is G.DocString ? this.MapToString((G.DocString) step.Argument) : null,
                Keyword = this.MapToKeyword(step.Keyword),
                NativeKeyword = step.Keyword,
                Name = step.Text,
                TableArgument = step.Argument is G.DataTable ? this.MapToTable((G.DataTable) step.Argument) : null,
            };
        }

        public Keyword MapToKeyword(string keyword)
        {
            if (keyword == null)
            {
                return default(Keyword);
            }

            keyword = keyword.Trim();

            if (this.languageServices.WhenStepKeywords.Contains(keyword))
            {
                return Keyword.When;
            }

            if (this.languageServices.GivenStepKeywords.Contains(keyword))
            {
                return Keyword.Given;
            }

            if (this.languageServices.ThenStepKeywords.Contains(keyword))
            {
                return Keyword.Then;
            }

            if (this.languageServices.AndStepKeywords.Contains(keyword))
            {
                return Keyword.And;
            }

            if (this.languageServices.ButStepKeywords.Contains(keyword))
            {
                return Keyword.But;
            }

            throw new ArgumentOutOfRangeException("keyword");
        }

        public string MapToString(G.Tag tag)
        {
            return tag?.Name;
        }

        public Comment MapToComment(G.Comment comment)
        {
            if (comment == null)
            {
                return null;
            }

            return new Comment
            {
                Text = comment.Text.Trim(),
                Location = this.MapToLocation(comment.Location)
            };
        }

        public Location MapToLocation(G.Location location)
        {
            return location != null ? new Location { Column = location.Column, Line = location.Line } : null;
        }

        public Scenario MapToScenario(G.Scenario scenario, params string[] tagsToHide)
        {
            if (scenario == null)
            {
                return null;
            }

            var visibleTags = RetrieveOnlyVisibleTags(scenario.Tags, tagsToHide);

            return new Scenario
            {
                Description = scenario.Description ?? string.Empty,
                Location = this.MapToLocation(scenario.Location),
                Name = scenario.Name,
                Slug = scenario.Name.ToSlug(),
                Steps = scenario.Steps.Select(this.MapToStep).ToList(),
                Tags = visibleTags
            };
        }

        public Example MapToExample(G.Examples examples)
        {
            if (examples == null)
            {
                return null;
            }

            return new Example
            {
                Description = examples.Description,
                Name = examples.Name,
                TableArgument = this.MapToExampleTable(((G.IHasRows) examples).Rows),
                Tags = examples.Tags?.Select(this.MapToString).ToList()
            };
        }

        public ScenarioOutline MapToScenarioOutline(G.ScenarioOutline scenarioOutline, params string[] tagsToHide)
        {
            if (scenarioOutline == null)
            {
                return null;
            }

            var visibleTags = RetrieveOnlyVisibleTags(scenarioOutline.Tags, tagsToHide);

            return new ScenarioOutline
            {
                Description = scenarioOutline.Description ?? string.Empty,
                Examples = (scenarioOutline.Examples ?? new G.Examples[0]).Select(this.MapToExample).ToList(),
                Location = this.MapToLocation(scenarioOutline.Location),
                Name = scenarioOutline.Name,
                Slug = scenarioOutline.Name.ToSlug(),
                Steps = scenarioOutline.Steps.Select(this.MapToStep).ToList(),
                Tags = visibleTags
            };
        }

        public Scenario MapToScenario(G.Background background)
        {
            if (background == null)
            {
                return null;
            }

            return new Scenario
            {
                Description = background.Description ?? string.Empty,
                Location = this.MapToLocation(background.Location),
                Name = background.Name,
                Steps = background.Steps.Select(this.MapToStep).ToList(),
            };
        }

        public Feature MapToFeature(G.GherkinDocument gherkinDocument)
        {
            if (gherkinDocument == null)
            {
                return null;
            }

            var tagsToHide = string.IsNullOrEmpty(configuration.HideTags) ? null : configuration.HideTags.Split(';');

            var feature = new Feature();

            var background = gherkinDocument.Feature.Children.SingleOrDefault(c => c is G.Background) as G.Background;
            if (background != null)
            {
                feature.AddBackground(this.MapToScenario(background));
            }

            if (this.configuration.ShouldEnableComments)
            {
                feature.Comments.AddRange((gherkinDocument.Comments ?? new G.Comment[0]).Select(this.MapToComment));
            }

            feature.Description = gherkinDocument.Feature.Description ?? string.Empty;

            foreach (var featureElement in gherkinDocument.Feature.Children.Where(c => !(c is G.Background)))
            {
                feature.AddFeatureElement(this.MapToFeatureElement(featureElement, tagsToHide));
            }

            feature.Name = gherkinDocument.Feature.Name;


            List<string> tags = RetrieveOnlyVisibleTags(gherkinDocument.Feature.Tags, tagsToHide);
            feature.Tags.AddRange(tags);

            foreach (var comment in feature.Comments.ToArray())
            {
                // Find the related feature
                var relatedFeatureElement = feature.FeatureElements.LastOrDefault(x => x.Location.Line < comment.Location.Line);
                // Find the step to which the comment is related to
                if (relatedFeatureElement != null)
                {
                    var stepAfterComment = relatedFeatureElement.Steps.FirstOrDefault(x => x.Location.Line > comment.Location.Line);
                    if (stepAfterComment != null)
                    {
                        // Comment is before a step
                        comment.Type = CommentType.StepComment;
                        stepAfterComment.Comments.Add(comment);
                    }
                    else
                    {
                        // Comment is located after the last step
                        var stepBeforeComment = relatedFeatureElement.Steps.LastOrDefault(x => x.Location.Line < comment.Location.Line);
                        if (stepBeforeComment != null && stepBeforeComment == relatedFeatureElement.Steps.Last())
                        {

                            comment.Type = CommentType.AfterLastStepComment;
                            stepBeforeComment.Comments.Add(comment);
                        }
                    }
                }
            }

            foreach (var featureElement in feature.FeatureElements.ToArray())
            {
                featureElement.Feature = feature;
            }

            if (feature.Background != null)
            {
                feature.Background.Feature = feature;
            }

            feature.Language = gherkinDocument.Feature.Language ?? "en";

            return feature;
        }

        private IFeatureElement MapToFeatureElement(G.ScenarioDefinition sd,params string[] tagsToHide)
        {
            if (sd == null)
            {
                return null;
            }

            var scenario = sd as G.Scenario;
            if (scenario != null)
            {
                return this.MapToScenario(scenario, tagsToHide);
            }

            var scenarioOutline = sd as G.ScenarioOutline;
            if (scenarioOutline != null)
            {
                return this.MapToScenarioOutline(scenarioOutline, tagsToHide);
            }

            var background = sd as G.Background;
            if (background != null)
            {
                return this.MapToScenario(background);
            }

            throw new ArgumentException("Only arguments of type Scenario, ScenarioOutline and Background are supported.");
        }

        private List<string> RetrieveOnlyVisibleTags(IEnumerable<G.Tag> originalTags,params string[] tagsToHide)
        {
            var usableTags = new List<string>();
            foreach (var tag in originalTags)
            {
                var stringTag = this.MapToString(tag);

                if (tagsToHide != null && tagsToHide.Any(t => stringTag.Equals($"@{t}", StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                usableTags.Add(stringTag);
            }
            return usableTags;
        }
    }
}
