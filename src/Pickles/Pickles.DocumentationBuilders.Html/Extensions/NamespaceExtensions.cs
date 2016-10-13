//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NamespaceExtensions.cs" company="PicklesDoc">
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
using System.Xml.Linq;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.Extensions
{
    /// <summary>
    /// Extension Methods to work with namespaces.
    /// </summary>
    public static class NamespaceExtensions
    {
        /// <summary>
        /// Moves <paramref name="element"/> into namespace <paramref name="newNamespace"/>.
        /// </summary>
        /// <param name="element">The element that will be moved into a new namespace.</param>
        /// <param name="newNamespace">The new namespace for the element.</param>
        public static void MoveToNamespace(this XElement element, XNamespace newNamespace)
        {
            foreach (XElement el in element.DescendantsAndSelf())
            {
                el.Name = newNamespace.GetName(el.Name.LocalName);
            }
        }
    }
}
