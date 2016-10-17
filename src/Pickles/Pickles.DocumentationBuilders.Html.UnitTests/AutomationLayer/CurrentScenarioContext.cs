//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CurrentScenarioContext.cs" company="PicklesDoc">
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
using System.Collections;
using System.Xml.Linq;
using Autofac;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.HTML.AutomationLayer
{
    public class CurrentScenarioContext
    {
        private Hashtable Current { get; } = new Hashtable();

        public Feature Feature
        {
            get
            {
                if (Current.ContainsKey("Feature"))
                {
                    return Current["Feature"] as Feature;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Current["Feature"] = value;
            }
        }

        public IContainer Container { get; set; }

        public XElement Html
        {
            get
            {
                if (Current.ContainsKey("Html"))
                {
                    return Current["Html"] as XElement;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Current["Html"] = value;
            }
        }
    }
}