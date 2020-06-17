//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="VsTestElementExtensions.cs" company="PicklesDoc">
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
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.VsTest
{
    internal static class VsTestElementExtensions
    {
        private const string Failed = "failed";

        private static readonly XNamespace Ns = @"http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

        internal static string Feature(this XElement parentElement)
        {
            //// <UnitTest>
            ////     <TestMethod className="  featureName  " />
            //// </UnitTest>

            var className = parentElement
                .Element(Ns + "TestMethod")
                ?.Attributes("className")
                ?.FirstOrDefault()
                ?.Value;

            if (className == null || !className.EndsWith("Feature", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var feature = className.Substring(0, className.LastIndexOf("Feature", StringComparison.OrdinalIgnoreCase)).Split('.').Last();

            return feature;
        }

        internal static string Name(this XElement scenario)
        {
            //// <UnitTest name="   the name of the scenario   ">

            return scenario.Attribute("name")?.Value ?? String.Empty;
        }

        internal static IEnumerable<XElement> AllExecutionResults(this XDocument document)
        {
            //// TestRun/Results/UnitTestResult

            if (document?.Root == null)
            {
                return new XElement[0];
            }

            return document.Root.Descendants(Ns + "UnitTestResult");
        }

        /// <summary>
        /// Retrieves all potential scenarios in the test result file. "Potential" because
        /// there may be some regular unit tests included as well. They cause no problems, however.
        /// </summary>
        /// <param name="document">The test result file.</param>
        /// <returns>
        /// A sequence of <see cref="XElement"/> instances that are called "UnitTest".
        /// </returns>
        internal static IEnumerable<XElement> AllScenarios(this XDocument document)
        {
            //// TestRun/TestDefinitions/UnitTests that have a non-empty Description (which is the title of a Scenario).

            if (document?.Root == null)
            {
                return new XElement[0];
            }

            return document.Root.Descendants(Ns + "UnitTest");
        }

        internal static Guid ExecutionIdElement(this XElement scenario)
        {
            //// <UnitTest>
            ////   <Execution id="   the execution id guid   " />
            //// </UnitTest>

            var xElement = scenario?.Element(Ns + "Execution");

            return xElement != null ? new Guid(xElement.Attribute("id").Value) : Guid.Empty;
        }

        internal static IEnumerable<Guid> ExecutionIdElements(this IEnumerable<XElement> scenarios)
        {
            return scenarios.Select(ExecutionIdElement);
        }

        internal static TestResult Outcome(this XElement scenarioResult)
        {
            //// <UnitTestResult outcome="   the outcome   ">

            var outcomeAttribute = scenarioResult.Attribute("outcome")?.Value ?? Failed;

            switch (outcomeAttribute.ToLowerInvariant())
            {
                case "passed":
                    return TestResult.Passed;
                case Failed:
                    return TestResult.Failed;
                default:
                    return TestResult.Inconclusive;
            }
        }

        internal static Guid ExecutionIdAttribute(this XElement unitTestResult)
        {
            //// <UnitTestResult executionId="   the execution id guid   ">

            var executionIdAttribute = unitTestResult.Attribute("executionId");
            return executionIdAttribute != null ? new Guid(executionIdAttribute.Value) : Guid.Empty;
        }

        internal static bool BelongsToScenarioOutline(this XElement xmlScenario, ScenarioOutline scenarioOutline)
        {
            return xmlScenario.Name().ToUpperInvariant().StartsWith(TransformName(scenarioOutline.Name));
        }

        internal static bool BelongsToScenario(this XElement xmlScenario, Scenario scenario)
        {
            return xmlScenario.Name().ToUpperInvariant() == TransformName(scenario.Name);
        }

        private static string TransformName(string name)
        {
            return SpecFlowNameMapping.Build(name).ToUpperInvariant();
        }
    }
}