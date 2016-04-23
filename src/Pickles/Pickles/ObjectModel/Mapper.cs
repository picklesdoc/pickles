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
using AutoMapper;
using AutoMapper.Mappers;
using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.ObjectModel
{
    public class Mapper : IDisposable
    {
        private readonly MappingEngine mapper;

        public Mapper(string featureLanguage = LanguageServices.DefaultLanguage)
        {
            var configurationStore = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);

            this.mapper = new MappingEngine(configurationStore);

            configurationStore.CreateMap<string, Keyword>().ConvertUsing(new KeywordResolver(featureLanguage));

            configurationStore.CreateMap<G.TableCell, string>().ConvertUsing(this.MapToString);
            configurationStore.CreateMap<G.TableRow, TableRow>().ConstructUsing(this.MapToTableRow);
            configurationStore.CreateMap<G.DataTable, Table>().ConvertUsing(this.MapToTable);
            configurationStore.CreateMap<G.DocString, string>().ConvertUsing(this.MapToString);
            configurationStore.CreateMap<G.Location, Location>().ConvertUsing(this.MapToLocation);
            configurationStore.CreateMap<G.Comment, Comment>().ConvertUsing(this.MapToComment);
            configurationStore.CreateMap<G.Step, Step>().ConvertUsing(this.MapToStep);
            configurationStore.CreateMap<G.Tag, string>().ConvertUsing(this.MapToString);
            configurationStore.CreateMap<G.Scenario, Scenario>().ConvertUsing(this.MapToScenario);
            configurationStore.CreateMap<IEnumerable<G.TableRow>, Table>().ConvertUsing(this.MapToTable);
            configurationStore.CreateMap<G.Examples, Example>().ConvertUsing(this.MapToExample);
            configurationStore.CreateMap<G.ScenarioOutline, ScenarioOutline>().ConvertUsing(this.MapToScenarioOutline);

            configurationStore.CreateMap<G.Background, Scenario>()
                .ForMember(t => t.Description, opt => opt.NullSubstitute(string.Empty));

            configurationStore.CreateMap<G.ScenarioDefinition, IFeatureElement>().ConvertUsing(
                sd =>
                {
                    if (sd == null)
                    {
                        return null;
                    }

                    var scenario = sd as G.Scenario;
                    if (scenario != null)
                    {
                        return this.mapper.Map<Scenario>(scenario);
                    }

                    var scenarioOutline = sd as G.ScenarioOutline;
                    if (scenarioOutline != null)
                    {
                        return this.mapper.Map<ScenarioOutline>(scenarioOutline);
                    }

                    var background = sd as G.Background;
                    if (background != null)
                    {
                        return this.mapper.Map<Scenario>(background);
                    }

                    throw new ArgumentException("Only arguments of type Scenario, ScenarioOutline and Background are supported.");
                });

            configurationStore.CreateMap<G.GherkinDocument, Feature>()
                .ForMember(t => t.Background, opt => opt.ResolveUsing(s => s.Feature.Children.SingleOrDefault(c => c is G.Background) as G.Background))
                .ForMember(t => t.Comments, opt => opt.ResolveUsing(s => s.Comments))
                .ForMember(t => t.Description, opt => opt.ResolveUsing(s => s.Feature.Description))
                .ForMember(t => t.FeatureElements, opt => opt.ResolveUsing(s => s.Feature.Children.Where(c => !(c is G.Background))))
                .ForMember(t => t.Name, opt => opt.ResolveUsing(s => s.Feature.Name))
                .ForMember(t => t.Tags, opt => opt.ResolveUsing(s => s.Feature.Tags))


                .ForMember(t => t.Description, opt => opt.NullSubstitute(string.Empty))
                .AfterMap(
                    (sourceFeature, targetFeature) =>
                        {
                            foreach (var comment in targetFeature.Comments.ToArray())
                            {
                                // Find the related feature
                                var relatedFeatureElement = targetFeature.FeatureElements.LastOrDefault(x => x.Location.Line < comment.Location.Line);
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

                            foreach (var featureElement in targetFeature.FeatureElements.ToArray())
                            {
                                featureElement.Feature = targetFeature;
                            }

                            if (targetFeature.Background != null)
                            {
                                targetFeature.Background.Feature = targetFeature;
                            }
                        });
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
                DataRows = tableRows.Skip(1).Select(this.MapToTableRow).ToList()
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
                Location = this.mapper.Map<Location>(step.Location),
                DocStringArgument = step.Argument is G.DocString ? this.mapper.Map<string>((G.DocString) step.Argument) : null,
                Keyword = this.mapper.Map<Keyword>(step.Keyword),
                NativeKeyword = step.Keyword,
                Name = step.Text,
                TableArgument = step.Argument is G.DataTable ? this.mapper.Map<Table>((G.DataTable) step.Argument) : null,
            };
        }

        public Keyword MapToKeyword(string keyword)
        {
            return this.mapper.Map<Keyword>(keyword);
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
                Location = this.mapper.Map<Location>(comment.Location)
            };
        }

        public Location MapToLocation(G.Location location)
        {
            return location != null ? new Location { Column = location.Column, Line = location.Line } : null;
        }

        public Scenario MapToScenario(G.Scenario scenario)
        {
            if (scenario == null)
            {
                return null;
            }

            return new Scenario
            {
                Description = scenario.Description ?? string.Empty,
                Location = this.MapToLocation(scenario.Location),
                Name = scenario.Name,
                Steps = scenario.Steps.Select(this.MapToStep).ToList(),
                Tags = scenario.Tags.Select(this.MapToString).ToList()
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
                TableArgument = this.MapToTable(((G.IHasRows) examples).Rows)
            };
        }

        public ScenarioOutline MapToScenarioOutline(G.ScenarioOutline scenarioOutline)
        {
            if (scenarioOutline == null)
            {
                return null;
            }

            return new ScenarioOutline
            {
                Description = scenarioOutline.Description ?? string.Empty,
                Examples = (scenarioOutline.Examples ?? new G.Examples[0]).Select(this.MapToExample).ToList(),
                Location = this.MapToLocation(scenarioOutline.Location),
                Name = scenarioOutline.Name,
                Steps = scenarioOutline.Steps.Select(this.MapToStep).ToList(),
                Tags = scenarioOutline.Tags.Select(this.MapToString).ToList()
            };
        }

        public Scenario MapToScenario(G.Background background)
        {
            return this.mapper.Map<Scenario>(background);
        }

        public Feature MapToFeature(G.GherkinDocument gherkinDocument)
        {
            return this.mapper.Map<Feature>(gherkinDocument);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.mapper.Dispose();
            }
        }
    }
}
