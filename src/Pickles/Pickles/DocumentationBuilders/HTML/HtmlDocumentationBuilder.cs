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
using System.IO.Abstractions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using NLog;
using NGenerics.DataStructures.Trees;
using NGenerics.Patterns.Visitor;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlDocumentationBuilder : IDocumentationBuilder
    {
      private static readonly Logger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly Configuration configuration;
        private readonly HtmlDocumentFormatter htmlDocumentFormatter;
        private readonly HtmlResourceWriter htmlResourceWriter;

        private readonly IFileSystem fileSystem;

        public HtmlDocumentationBuilder(
            Configuration configuration,
            HtmlDocumentFormatter htmlDocumentFormatter,
            HtmlResourceWriter htmlResourceWriter,
            IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.htmlDocumentFormatter = htmlDocumentFormatter;
            this.htmlResourceWriter = htmlResourceWriter;
            this.fileSystem = fileSystem;
        }

        #region IDocumentationBuilder Members

        public void Build(GeneralTree<INode> features)
        {
            if (log.IsInfoEnabled)
            {
              log.Info("Writing HTML to {0}", this.configuration.OutputFolder.FullName);
            }

            this.htmlResourceWriter.WriteTo(this.configuration.OutputFolder.FullName);


            var actionVisitor = new ActionVisitor<INode>(node =>
                                                                          {
                                                                              if (node.IsIndexMarkDownNode())
                                                                              {
                                                                                  return;
                                                                              }

                                                                              string nodePath =
                                                                                  this.fileSystem.Path.Combine(
                                                                                      this.configuration.OutputFolder.
                                                                                          FullName,
                                                                                      node.RelativePathFromRoot);
                                                                              string htmlFilePath;

                                                                              if (node.IsContent)
                                                                              {
                                                                                  htmlFilePath =
                                                                                      nodePath.Replace(
                                                                                          this.fileSystem.Path.GetExtension(nodePath),
                                                                                          ".html");
                                                                              }
                                                                              else
                                                                              {
                                                                                  this.fileSystem.Directory.CreateDirectory(nodePath);

                                                                                  htmlFilePath = this.fileSystem.Path.Combine(nodePath,
                                                                                                              "index.html");
                                                                              }

                                                                              using (
                                                                                  var writer =
                                                                                      new System.IO.StreamWriter(htmlFilePath,
                                                                                                       false,
                                                                                                       Encoding.UTF8))
                                                                              {
                                                                                  XDocument document =
                                                                                      this.htmlDocumentFormatter.Format(
                                                                                          node, features,
                                                                                          this.configuration.FeatureFolder);
                                                                                  document.Save(writer);
                                                                                  writer.Close();
                                                                              }
                                                                          });
            if (features != null)
                features.AcceptVisitor(actionVisitor);
        }

        #endregion
    }
}