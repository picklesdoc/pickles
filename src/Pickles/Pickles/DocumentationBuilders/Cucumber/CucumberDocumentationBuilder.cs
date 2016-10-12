//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="JSONDocumentationBuilder.cs" company="PicklesDoc">
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

using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NGenerics.DataStructures.Trees;
using NGenerics.Patterns.Visitor;
using NLog;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Cucumber
{
	using System.Linq;

	public class CucumberDocumentationBuilder : IDocumentationBuilder
	{
		#region Private Constants
		public const string CucumberFileName = @"cucumberResult.json";
		#endregion

		#region Private Fields
		private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType?.Name);
		private readonly IConfiguration configuration;
		private readonly IFileSystem fileSystem;
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the output file path.
		/// </summary>
		/// <value>
		/// The output file path.
		/// </value>
		public string OutputFilePath => this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, CucumberFileName);
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CucumberDocumentationBuilder" /> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="fileSystem">The file system.</param>
		public CucumberDocumentationBuilder(IConfiguration configuration, IFileSystem fileSystem)
		{
			this.configuration = configuration;
			this.fileSystem = fileSystem;
		}
		#endregion

		#region IDocumentationBuilder Members

		/// <summary>
		/// Builds the specified features.
		/// </summary>
		/// <param name="features">The features.</param>
		public void Build(GeneralTree<INode> features)
		{
			if (Log.IsInfoEnabled)
			{
				Log.Info("Writing Cucumber to {0}", this.configuration.OutputFolder.FullName);
			}

			List<Feature> featuresToFormat = new List<Feature>();

			ActionVisitor<INode> actionVisitor = new ActionVisitor<INode>(node =>
			{

				FeatureNode featureTreeNode = node as FeatureNode;
				if (featureTreeNode != null)
				{
					featuresToFormat.Add(featureTreeNode.Feature);
				}
			});

			features.AcceptVisitor(actionVisitor);

			this.CreateFile(this.OutputFilePath, this.GenerateJson(featuresToFormat));
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Generates the json.
		/// </summary>
		/// <param name="features">The features.</param>
		/// <returns></returns>
		private string GenerateJson(List<Feature> features)
		{
			var toOutPut = features.Select(f => new
			{
				keyword = "Feature",
				name = f.Name,
				tags = f.Tags.Select(t => new { name = t }),
				line = 1,
				elements = f.FeatureElements.Select(fe =>
					new
					{
						keyword = fe is Scenario ? "Scenario" : "Scenario Outline",
						name = fe.Name,
						line = fe.Location.Line,
						type = fe is Scenario ? "scenario" : "scenario_outline",
						tags = fe.Tags.Select(t => new { name = t }),
						steps = fe.Steps.Select(s => new
						{
							keyword = s.Keyword,
							name = s.Name,
							line = s.Location.Line,
							result = new
							{
								status = fe.Result.ToString().ToLowerInvariant(),
								duration = 1
							}
						})
					}
					  ),

			});


			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				Converters = new List<JsonConverter> { new StringEnumConverter() }
			};

			return JsonConvert.SerializeObject(toOutPut, Formatting.Indented, settings);
		}

		/// <summary>
		/// Creates the file.
		/// </summary>
		/// <param name="outputFolderName">Name of the output folder.</param>
		/// <param name="jsonToWrite">The json to write.</param>
		private void CreateFile(string outputFolderName, string jsonToWrite)
		{
			using (StreamWriter writer = this.fileSystem.File.CreateText(outputFolderName))
			{
				writer.Write(jsonToWrite);
				writer.Close();
			}
		}
		#endregion
	}
}
