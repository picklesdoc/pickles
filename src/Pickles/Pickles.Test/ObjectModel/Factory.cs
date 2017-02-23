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
        private const G.Location AnyLocation = null;

        internal Mapper CreateMapper(string defaultLanguage = "en")
        {
            var mapper = new Mapper(defaultLanguage);
            return mapper;
        }

        internal Mapper CreateMapper(IConfiguration configuration, string defaultLanguage = "en")
        {
            var mapper = new Mapper(configuration, defaultLanguage);
            return mapper;
        }

        internal G.TableCell CreateGherkinTableCell(string cellValue)
        {
            return new G.TableCell(AnyLocation, cellValue);
        }

        internal G.DocString CreateDocString(string docString = null)
        {
            return new G.DocString(
                AnyLocation,
                null,
                docString ?? @"My doc string line 1
My doc string line 2");
        }

        internal G.TableRow CreateGherkinTableRow(params string[] cellValues)
        {
            return new G.TableRow(
                AnyLocation,
                cellValues.Select(this.CreateGherkinTableCell).ToArray());
        }

        internal G.DataTable CreateGherkinDataTable(IEnumerable<string[]> rows)
        {
            return new G.DataTable(rows.Select(this.CreateGherkinTableRow).ToArray());
        }

        internal G.Step CreateStep(string keyword, string text)
        {
            return new G.Step(AnyLocation, keyword, text, null);
        }

        internal G.Step CreateStep(string keyword, string text, int locationLine, int locationColumn)
        {
            var step =  new G.Step(this.CreateLocation(locationLine, locationColumn), keyword, text, null);
            return step;
        }

        internal G.Step CreateStep(string keyword, string text, string docString)
        {
            return new G.Step(AnyLocation, keyword, text, this.CreateDocString(docString));
        }

        internal G.Step CreateStep(string keyword, string text, IEnumerable<string[]> rows)
        {
            return new G.Step(AnyLocation, keyword, text, this.CreateGherkinDataTable(rows));
        }

        internal G.Tag CreateTag(string tag)
        {
            return new G.Tag(AnyLocation, tag);
        }

        internal G.Location CreateLocation(int line, int column)
        {
            return new G.Location(line, column);
        }

        internal G.Comment CreateComment(string comment, int locationLine, int locationColumn)
        {
            return new G.Comment(this.CreateLocation(locationLine, locationColumn), comment);
        }

        internal G.Scenario CreateScenario(string[] tags, string name, string description, G.Step[] steps, G.Location location = null)
        {
            G.Scenario scenario = new G.Scenario(
                tags.Select(this.CreateTag).ToArray(),
                location ?? AnyLocation,
                "Scenario",
                name,
                description,
                steps);
            return scenario;
        }

        internal G.Examples CreateExamples(string name, string description, string[] headerCells, IEnumerable<string[]> exampleRows, string[] tags = null)
        {
            var examples = new G.Examples(
                tags?.Select(this.CreateTag).ToArray(),
                AnyLocation,
                "Examples",
                name,
                description,
                this.CreateGherkinTableRow(headerCells),
                exampleRows.Select(this.CreateGherkinTableRow).ToArray());

            return examples;
        }

        internal G.ScenarioOutline CreateScenarioOutline(string[] tags, string name, string description, G.Step[] steps, G.Examples[] examples)
        {
            G.ScenarioOutline scenarioOutline = new G.ScenarioOutline(
                tags.Select(this.CreateTag).ToArray(),
                AnyLocation,
                "Scenario",
                name,
                description,
                steps,
                examples);
            return scenarioOutline;
        }

        internal G.Background CreateBackground(string name, string description, G.Step[] steps)
        {
            G.Background background = new G.Background(
                AnyLocation,
                "Background",
                name,
                description,
                steps);
            return background;
        }

        internal G.GherkinDocument CreateGherkinDocument(string name, string description, string[] tags = null, G.Background background = null, G.ScenarioDefinition[] scenarioDefinitions = null, G.Comment[] comments = null, G.Location location = null)
        {
            var nonNullScenarioDefinitions = scenarioDefinitions ?? new G.ScenarioDefinition[0];
            return new G.GherkinDocument(
                new G.Feature(
                    (tags ?? new string[0]).Select(this.CreateTag).ToArray(),
                    location,
                    null,
                    "Feature",
                    name,
                    description,
                    background != null ? new G.ScenarioDefinition[] { background }.Concat(nonNullScenarioDefinitions).ToArray() : nonNullScenarioDefinitions),
                comments);
        }
    }
}
