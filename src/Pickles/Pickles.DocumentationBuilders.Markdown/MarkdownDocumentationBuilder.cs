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
using System.IO;
using System.IO.Abstractions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    public class MarkdownDocumentationBuilder : IDocumentationBuilder
    {
        private readonly Stylist style = new Stylist();
        private readonly IFileSystem fileSystem;
        private readonly IConfiguration configuration;

        public MarkdownDocumentationBuilder(IFileSystem fileSystem, IConfiguration configuration)
        {
            this.fileSystem = fileSystem;
            this.configuration = configuration;
        }

        public void Build(Tree featureTree)
        {
            var documentation = new Documentation(featureTree);

            WriteContentToFile(TargetFile(), documentation.CurrentPage);
        }

        // TODO: new class handles file system interaction
        private void WriteContentToFile(string filePath, string content)
        {
            using (var file = fileSystem.FileInfo.FromFileName(filePath).CreateText())
            {
                file.Write(content);

                file.Close();
            }
        }

        private string TargetFile()
        {
            var fileName = "features.md";

            string defaultOutputFile = string.Empty;
            if (configuration.OutputFolder == null)
            {
                defaultOutputFile = Path.Combine(@"C:\testing", fileName);
            }
            else
            {
                defaultOutputFile = Path.Combine(configuration.OutputFolder.FullName, fileName);
            }

            return defaultOutputFile;
        }
    }
}
