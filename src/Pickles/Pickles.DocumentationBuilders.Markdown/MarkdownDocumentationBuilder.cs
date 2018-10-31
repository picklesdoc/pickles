//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MarkdownDocumentationBuilder.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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

using PicklesDoc.Pickles.DataStructures;
using System.IO.Abstractions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    public class MarkdownDocumentationBuilder : IDocumentationBuilder
    {
        private readonly Stylist style = new Stylist();
        private readonly IFileSystem fileSystem;

        public MarkdownDocumentationBuilder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Build(Tree features)
        {
            var defaultOutputFile = @"c:\output\pickledFeatures.md";

            var content = MarkdownContent();

            WriteContentToFile(defaultOutputFile, content);
        }

        // TODO: new class handles file system interaction
        private void WriteContentToFile(string filePath, string content)
        {
            fileSystem.File.Create(filePath);

            fileSystem.File.AppendAllText(filePath, content);
        }

        // TODO: New class handles content, structure etc.
        private string MarkdownContent()
        {
            var content = (new TitleBlock(style)).Text;

            return content;
        }
    }
}
