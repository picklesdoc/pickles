//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DocumentationFormat.cs" company="PicklesDoc">
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

using System.ComponentModel;

namespace PicklesDoc.Pickles
{
    /// <summary>
    /// The supported documentation formats.
    /// </summary>
    public enum DocumentationFormat
    {
        /// <summary>
        /// Static HTML format without search.
        /// </summary>
        [Description("HTML")] Html,

        /// <summary>
        /// Word format.
        /// </summary>
        [Description("Microsoft Word OpenXML (.docx)")] Word,

        /// <summary>
        /// JSON format.
        /// </summary>
        [Description("Javascript Object Notation (JSON)")] Json,

        /// <summary>
        /// Excel format.
        /// </summary>
        [Description("Microsoft Excel OpenXML (.xlsx)")] Excel,

        /// <summary>
        /// Dynamic HTML format with search.
        /// </summary>
        [Description("HTML with search")] DHtml,

        /// <summary>
        /// Cucumber output JSON format
        /// </summary>
        [Description("Cucumber JSON export format")] Cucumber,

        /// <summary>
        /// Markdown output format
        /// </summary>
        [Description("Markdown output format")] Markdown



    }
}
