//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Factory.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.ObjectModel;
using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.Test.ObjectModel
{
    internal class Factory
    {
        private G.Location? AnyLocation = null;

        internal Mapper CreateMapper(string defaultLanguage = "en")
        {
            var mapper = this.CreateMapper(new Configuration(), defaultLanguage);

            return mapper;
        }

        internal Mapper CreateMapper(IConfiguration configuration, string defaultLanguage = "en")
        {
            var languageServices = new LanguageServices(defaultLanguage);
            var mapper = new Mapper(configuration, languageServices);
            return mapper;
        }

        internal G.TableCell CreateGherkinTableCell(string cellValue)
        {
            // Ici, on récupère la valeur de AnyLocation ou un Location par défaut (ex: default(G.Location))
            var location = AnyLocation ?? default(G.Location);
            return new G.TableCell(location, cellValue);
        }

        internal G.DocString CreateDocString(string docString = null)
        {
            var location = AnyLocation ?? default(G.Location);
            return new G.DocString(
                location,
                null,
                docString ?? @"My doc string line 1
My doc string line 2");
        }

        internal G.TableRow CreateGherkinTableRow(params string[] cellValues)
        {
            var location = AnyLocation ?? default(G.Location);
            return new G.TableRow(
                location,
                cellValues.Select(this.CreateGherkinTableCell).ToArray());
        }

        internal G.DataTable CreateGherkinDataTable(IEnumerable<string[]> rows)
        {
            return new G.DataTable(rows.Select(this.CreateGherkinTableRow).ToList());
        }

        internal G.Step CreateStep(string keyword, string text)
        {
            var location = AnyLocation ?? default(G.Location);
            return new G.Step(location, keyword, Gherkin.StepKeywordType.Unspecified, text, null);
        }

        internal G.Step CreateStep(string keyword, string text, int locationLine, int locationColumn)
        {
            var step =  new G.Step(this.CreateLocation(locationLine, locationColumn), keyword, Gherkin.StepKeywordType.Unspecified, text, null);
            return step;
        }

        internal G.Step CreateStep(string keyword, string text, string docString)
        {
            var location = AnyLocation ?? default(G.Location);
            return new G.Step(location, keyword, Gherkin.StepKeywordType.Unspecified, text, this.CreateDocString(docString));
        }

        internal G.Step CreateStep(string keyword, string text, IEnumerable<string[]> rows)
        {
            var location = AnyLocation ?? default(G.Location);
            return new G.Step(location, keyword, Gherkin.StepKeywordType.Unspecified, text, this.CreateGherkinDataTable(rows));
        }

        internal G.Tag CreateTag(string tag)
        {
            var location = AnyLocation ?? default(G.Location);
            return new G.Tag(location, tag);
        }

        internal G.Location CreateLocation(int line, int column)
        {
            return new G.Location(line, column);
        }

        internal G.Comment CreateComment(string comment, int locationLine, int locationColumn)
        {
            return new G.Comment(this.CreateLocation(locationLine, locationColumn), comment);
        }

        internal G.Scenario CreateScenario(string[] tags, string name, string description, G.Step[] steps, G.Location? location = null)
        {
            var location2 = location ?? default(G.Location);
            G.Scenario scenario = new G.Scenario(
                tags.Select(this.CreateTag).ToArray(),
                location2,
                "Scenario",
                name,
                description,
                steps,
                null);
            return scenario;
        }

        internal G.Examples CreateExamples(string name, string description, string[] headerCells, IEnumerable<string[]> exampleRows, string[] tags = null)
        {

            var location = AnyLocation ?? default(G.Location);
            var examples = new G.Examples(
                tags?.Select(this.CreateTag).ToArray(),
                location,
                "Examples",
                name,
                description,
                this.CreateGherkinTableRow(headerCells),
                exampleRows.Select(this.CreateGherkinTableRow).ToArray());

            return examples;
        }

        internal G.Scenario CreateScenarioOutline(string[] tags, string name, string description, G.Step[] steps, G.Examples[] examples)
        {
            var location = AnyLocation ?? default(G.Location);
            G.Scenario scenarioOutline = new G.Scenario(
                tags.Select(this.CreateTag).ToArray(),
                location,
                "Scenario",
                name,
                description,
                steps,
                examples);
            return scenarioOutline;
        }

        internal G.Background CreateBackground(string name, string description, G.Step[] steps)
        {
            var location = AnyLocation ?? default(G.Location);
            G.Background background = new G.Background(
                location,
                "Background",
                name,
                description,
                steps);
            return background;
        }

        internal G.GherkinDocument CreateGherkinDocument(string name, string description, string[] tags = null, G.Background background = null, G.IHasLocation [] scenarioDefinitions = null, G.Comment[] comments = null, G.Location? location = null, string language = null)
        {
            var nonNullScenarioDefinitions = scenarioDefinitions ?? new G.IHasLocation[0];

            var location2 = location ?? default(G.Location);
            return new G.GherkinDocument(
                new G.Feature(
                    (tags ?? new string[0]).Select(this.CreateTag).ToArray(),
                    location2,
                    language,
                    "Feature",
                    name,
                    description,
                    background != null ? new G.Background[] { background }.Concat(nonNullScenarioDefinitions).ToArray() : nonNullScenarioDefinitions),
                comments);
        }
    }
}
