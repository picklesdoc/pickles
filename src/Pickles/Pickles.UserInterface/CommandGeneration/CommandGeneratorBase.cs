//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CommandGeneratorBase.cs" company="PicklesDoc">
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
using System.IO;
using System.Linq;

using PicklesDoc.Pickles.UserInterface.Settings;

namespace PicklesDoc.Pickles.UserInterface.CommandGeneration
{
    public abstract class CommandGeneratorBase
    {
        public string Generate(MainModel model, string selectedLanguage)
        {
            return (from documentationFormat in model.DocumentationFormats
                    let outputDirectory = model.CreateDirectoryForEachOutputFormat
                                              ? model.OutputDirectory.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + documentationFormat
                                              : model.OutputDirectory
                                           ?.ToLowerInvariant()
                    select this.GenerateSingleCommandLine(model, outputDirectory, documentationFormat, selectedLanguage))
                   .Aggregate(string.Empty, (c, n) => c + Environment.NewLine + n)
                   .Trim();
        }

        protected abstract string GenerateSingleCommandLine(
            MainModel model,
            string outputDirectory,
            DocumentationFormat documentationFormat,
            string selectedLanguage);
    }
}