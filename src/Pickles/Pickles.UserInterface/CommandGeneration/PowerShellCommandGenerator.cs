//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PowerShellCommandGenerator.cs" company="PicklesDoc">
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
using System.Text;

using PicklesDoc.Pickles.UserInterface.Settings;

namespace PicklesDoc.Pickles.UserInterface.CommandGeneration
{
    public class PowerShellCommandGenerator : CommandGeneratorBase
    {
        /// <inheritdoc />
        protected override string GenerateSingleCommandLine(
            MainModel model,
            string outputDirectory,
            DocumentationFormat documentationFormat,
            string selectedLanguage)
        {
            var result = new StringBuilder("Pickle-Features")
                         .AppendFormat(" -FeatureDirectory \"{0}\"", model.FeatureDirectory)
                         .AppendFormatIfNotEmpty(" -OutputDirectory \"{0}\"", outputDirectory)
                         .AppendFormatIfNotEmpty(" -SystemUnderTestName {0}", model.ProjectName)
                         .AppendFormatIfNotEmpty(" -SystemUnderTestVersion {0}", model.ProjectVersion);

            if (model.IncludeTestResults)
            {
                result.AppendFormatIfNotEmpty(" -TestResultsFile \"{0}\"", model.TestResultsFile);

                if (model.TestResultsFormat != TestResultsFormat.NUnit)
                {
                    result.AppendFormat(" -TestResultsFormat {0}", model.TestResultsFormat.ToString().ToLowerInvariant());
                }
            }

            if (!string.Equals(selectedLanguage, "en", StringComparison.OrdinalIgnoreCase))
            {
                result.AppendFormatIfNotEmpty(" -Language {0}", selectedLanguage);
            }

            if (documentationFormat != DocumentationFormat.Html)
            {
                result.AppendFormat(" -DocumentationFormat {0}", documentationFormat.ToString());
            }

            if (model.IncludeExperimentalFeatures)
            {
                result.Append(" -IncludeExperimentalFeatures");
            }

            if (!model.EnableComments)
            {
                result.Append(" -EnableComments false");
            }

            result.AppendFormatIfNotEmpty(" -ExcludeTags {0}", model.ExcludeTags);
            result.AppendFormatIfNotEmpty(" -HideTags {0}", model.HideTags);

            return result.ToString();
        }
    }
}