//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlModule.cs" company="PicklesDoc">
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

using Autofac;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HtmlDocumentationBuilder>().SingleInstance();
            builder.RegisterType<HtmlHeaderFormatter>().SingleInstance();
            builder.RegisterType<HtmlContentFormatter>().SingleInstance();
            builder.RegisterType<HtmlDocumentFormatter>().SingleInstance();
            builder.RegisterType<HtmlResourceSet>().SingleInstance();
            builder.RegisterType<HtmlImageResultFormatter>().SingleInstance();
            builder.RegisterType<HtmlIndexFormatter>().SingleInstance();
            builder.RegisterType<HtmlFeatureFormatter>().SingleInstance();
            builder.RegisterType<HtmlScenarioFormatter>().SingleInstance();
            builder.RegisterType<HtmlScenarioOutlineFormatter>().SingleInstance();
            builder.RegisterType<HtmlDescriptionFormatter>().SingleInstance();
            builder.RegisterType<HtmlStepFormatter>().SingleInstance();
            builder.RegisterType<HtmlTableFormatter>().SingleInstance();
            builder.RegisterType<HtmlMultilineStringFormatter>().SingleInstance();
        }
    }
}