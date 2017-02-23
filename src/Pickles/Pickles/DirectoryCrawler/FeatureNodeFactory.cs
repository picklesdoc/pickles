//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureNodeFactory.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Xml.Linq;

using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DirectoryCrawler
{
    public class FeatureNodeFactory
    {
        private readonly FeatureParser featureParser;
        private readonly HtmlMarkdownFormatter htmlMarkdownFormatter;
        private readonly RelevantFileDetector relevantFileDetector;
        private readonly IFileSystem fileSystem;

        public FeatureNodeFactory(RelevantFileDetector relevantFileDetector, FeatureParser featureParser, HtmlMarkdownFormatter htmlMarkdownFormatter, IFileSystem fileSystem)
        {
            this.relevantFileDetector = relevantFileDetector;
            this.featureParser = featureParser;
            this.htmlMarkdownFormatter = htmlMarkdownFormatter;
            this.fileSystem = fileSystem;
        }

        public INode Create(FileSystemInfoBase root, FileSystemInfoBase location)
        {
            string relativePathFromRoot = root == null ? @".\" : PathExtensions.MakeRelativePath(root, location, this.fileSystem);

            var directory = location as DirectoryInfoBase;
            if (directory != null)
            {
                return new FolderNode(directory, relativePathFromRoot);
            }

            var file = location as FileInfoBase;
            if (this.relevantFileDetector.IsFeatureFile(file))
            {
                Feature feature = this.featureParser.Parse(file.FullName);
                return feature != null ? new FeatureNode(file, relativePathFromRoot, feature) : null;
            }
            else if (this.relevantFileDetector.IsMarkdownFile(file))
            {
                XElement markdownContent = this.htmlMarkdownFormatter.Format(this.fileSystem.File.ReadAllText(file.FullName));
                return new MarkdownNode(file, relativePathFromRoot, markdownContent);
            }
            else if (this.relevantFileDetector.IsImageFile(file))
            {
                return new ImageNode(file, relativePathFromRoot);
            }

            throw new InvalidOperationException("Cannot create an IItemNode-derived object for " + file.FullName);
        }
    }
}
