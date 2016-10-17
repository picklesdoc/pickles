//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Hooks.cs" company="PicklesDoc">
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
using System.Linq;

using Autofac;

using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests.AutomationLayer
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext scenarioContext;

        public Hooks(CurrentScenarioContext currentScenarioContext, ScenarioContext scenarioContext)
        {
            this.CurrentScenarioContext = currentScenarioContext;
            this.scenarioContext = scenarioContext;
        }

        private CurrentScenarioContext CurrentScenarioContext { get; }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Runner).Assembly);
            builder.RegisterModule<PicklesModule>();
            this.CurrentScenarioContext.Container = builder.Build();

            if (this.scenarioContext.ScenarioInfo.Tags.Contains("enableExperimentalFeatures"))
            {
                var configuration = this.CurrentScenarioContext.Container.Resolve<IConfiguration>();
                configuration.EnableExperimentalFeatures();
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var container = this.CurrentScenarioContext.Container;

            container?.Dispose();

            this.CurrentScenarioContext.Container = null;
        }
    }
}
