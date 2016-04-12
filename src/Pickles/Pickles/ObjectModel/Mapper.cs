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

            configurationStore.CreateMap<G.TableCell, string>()
                .ConstructUsing(cell => cell.Value);

            configurationStore.CreateMap<G.TableRow, TableRow>()
                .ConstructUsing(row => new TableRow(row.Cells.Select(this.mapper.Map<string>)));

            configurationStore.CreateMap<G.DataTable, Table>()
                .ForMember(t => t.HeaderRow, opt => opt.MapFrom(dt => dt.Rows.Take(1).Single()))
                .ForMember(t => t.DataRows, opt => opt.MapFrom(dt => dt.Rows.Skip(1)));

            configurationStore.CreateMap<G.DocString, string>().ConstructUsing(docString => docString.Content);

            configurationStore.CreateMap<G.Location, Location>()
                .ForMember(t => t.Column, opt => opt.MapFrom(s => s.Column))
                .ForMember(t => t.Line, opt => opt.MapFrom(s => s.Line));

            configurationStore.CreateMap<G.Comment, Comment>()
                .ForMember(t => t.Text, opt => opt.MapFrom(s => s.Text))
                .ForMember(t => t.Location, opt => opt.MapFrom(s => s.Location))
                .AfterMap(
                    (sourceComment, targetComment) =>
                    {
                        targetComment.Text = targetComment.Text.Trim();
                    }
                );

            configurationStore.CreateMap<G.Step, Step>()
                .ForMember(t => t.NativeKeyword, opt => opt.MapFrom(s => s.Keyword))
                .ForMember(t => t.Name, opt => opt.MapFrom(s => s.Text))
                .ForMember(t => t.Location, opt => opt.MapFrom(s => s.Location))
                .ForMember(t => t.DocStringArgument, opt => opt.MapFrom(s => s.Argument is G.DocString ? s.Argument : null))
                .ForMember(t => t.TableArgument, opt => opt.MapFrom(s => s.Argument is G.DataTable ? s.Argument : null));

            configurationStore.CreateMap<G.Tag, string>()
                .ConstructUsing(tag => tag.Name);

            configurationStore.CreateMap<G.Scenario, Scenario>()
                .ForMember(t => t.Description, opt => opt.NullSubstitute(string.Empty))
                .ForMember(t => t.Location, opt => opt.MapFrom(s => s.Location));

            configurationStore.CreateMap<IEnumerable<G.TableRow>, Table>()
                .ForMember(t => t.HeaderRow, opt => opt.MapFrom(s => s.Take(1).Single()))
                .ForMember(t => t.DataRows, opt => opt.MapFrom(s => s.Skip(1)));

            configurationStore.CreateMap<G.Examples, Example>()
                .ForMember(t => t.TableArgument, opt => opt.MapFrom(s => ((G.IHasRows)s).Rows));

            configurationStore.CreateMap<G.ScenarioOutline, ScenarioOutline>()
                .ForMember(t => t.Description, opt => opt.NullSubstitute(string.Empty))
                .ForMember(t => t.Location, opt => opt.MapFrom(s => s.Location));

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
            return this.mapper.Map<string>(cell);
        }

        public TableRow MapToTableRow(G.TableRow tableRow)
        {
            return this.mapper.Map<TableRow>(tableRow);
        }

        public Table MapToTable(G.DataTable dataTable)
        {
            return this.mapper.Map<Table>(dataTable);
        }

        public string MapToString(G.DocString docString)
        {
            return this.mapper.Map<string>(docString);
        }

        public Step MapToStep(G.Step step)
        {
            return this.mapper.Map<Step>(step);
        }

        public Keyword MapToKeyword(string keyword)
        {
            return this.mapper.Map<Keyword>(keyword);
        }

        public string MapToString(G.Tag tag)
        {
            return this.mapper.Map<string>(tag);
        }

        public Comment MapToComment(G.Comment comment)
        {
            return this.mapper.Map<Comment>(comment);
        }

        public Location MapToLocation(G.Location location)
        {
            return this.mapper.Map<Location>(location);
        }

        public Scenario MapToScenario(G.Scenario scenario)
        {
            return this.mapper.Map<Scenario>(scenario);
        }

        public Example MapToExample(G.Examples examples)
        {
            return this.mapper.Map<Example>(examples);
        }

        public ScenarioOutline MapToScenarioOutline(G.ScenarioOutline scenarioOutline)
        {
            return this.mapper.Map<ScenarioOutline>(scenarioOutline);
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
