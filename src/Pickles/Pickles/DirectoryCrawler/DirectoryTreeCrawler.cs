#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using NGenerics.DataStructures.Trees;
using NLog;

namespace PicklesDoc.Pickles.DirectoryCrawler
{
    public class DirectoryTreeCrawler
    {
      private static readonly Logger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
      private readonly FeatureNodeFactory featureNodeFactory;

        private readonly IFileSystem fileSystem;

        private readonly RelevantFileDetector relevantFileDetector;

        public DirectoryTreeCrawler(RelevantFileDetector relevantFileDetector, FeatureNodeFactory featureNodeFactory, IFileSystem fileSystem)
        {
            this.relevantFileDetector = relevantFileDetector;
            this.featureNodeFactory = featureNodeFactory;
          this.fileSystem = fileSystem;
        }

        public GeneralTree<INode> Crawl(string directory)
        {
            return this.Crawl(this.fileSystem.DirectoryInfo.FromDirectoryName(directory), null);
        }

        public GeneralTree<INode> Crawl(DirectoryInfoBase directory)
        {
            return this.Crawl(directory, null);
        }

        private GeneralTree<INode> Crawl(DirectoryInfoBase directory, INode rootNode)
        {
          INode currentNode =
              this.featureNodeFactory.Create(rootNode != null ? rootNode.OriginalLocation : null, directory);

          if (rootNode == null)
          {
            rootNode = currentNode;
          }

          var tree = new GeneralTree<INode>(currentNode);

          var filesAreFound = this.CollectFiles(directory, rootNode, tree);

          var directoriesAreFound = this.CollectDirectories(directory, rootNode, tree);

          if (!filesAreFound && !directoriesAreFound)
          {
            return null;
          }

          return tree;
        }

        private bool CollectDirectories(DirectoryInfoBase directory, INode rootNode, GeneralTree<INode> tree)
      {
        List<GeneralTree<INode>> collectedNodes = new List<GeneralTree<INode>>();

        foreach (DirectoryInfoBase subDirectory in directory.GetDirectories().OrderBy(di => di.Name))
        {
          GeneralTree<INode> subTree = this.Crawl(subDirectory, rootNode);
          if (subTree != null)
          {
            collectedNodes.Add(subTree);
          }
        }

        foreach (var node in collectedNodes)
        {
          tree.Add(node);
        }

        return collectedNodes.Count > 0;
      }

        private bool CollectFiles(DirectoryInfoBase directory, INode rootNode, GeneralTree<INode> tree)
      {
        List<INode> collectedNodes = new List<INode>();

        foreach (FileInfoBase file in directory.GetFiles().Where(file => this.relevantFileDetector.IsRelevant(file)))
        {
          INode node = null;
          try
          {
            node = this.featureNodeFactory.Create(rootNode.OriginalLocation, file);
          }
          catch (Exception)
          {
            if (log.IsWarnEnabled)
            {
              log.Warn("The file, {0}, will be ignored because it could not be read in properly", file.FullName);
            }
          }

          if (node != null)
          {
            collectedNodes.Add(node);
          }
        }

        foreach (var node in OrderFileNodes(collectedNodes))
        {
          tree.Add(node);
        }

        return collectedNodes.Count > 0;
      }

      private static IEnumerable<INode> OrderFileNodes(List<INode> collectedNodes)
      {
        var indexFiles = collectedNodes.Where(node => node.OriginalLocation.Name.StartsWith("index", StringComparison.InvariantCultureIgnoreCase));
        var otherFiles = collectedNodes.Except(indexFiles);

        return indexFiles.OrderBy(node => node.Name).Concat(otherFiles.OrderBy(node => node.Name));
      }
    }
}