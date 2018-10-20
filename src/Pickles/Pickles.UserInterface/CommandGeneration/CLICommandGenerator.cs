//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CLICommandGenerator.cs" company="PicklesDoc">
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
    public class CLICommandGenerator : CommandGeneratorBase
    {
        protected override string GenerateSingleCommandLine(
            MainModel model,
            string outputDirectory,
            DocumentationFormat documentationFormat,
            string selectedLanguage)
        {
            var result = new StringBuilder("pickles.exe");
            result.AppendFormat(" --feature-directory=\"{0}\"", model.FeatureDirectory);
            result.AppendFormatIfNotEmpty(" --output-directory=\"{0}\"", outputDirectory);
            result.AppendFormatIfNotEmpty(" --system-under-test-name={0}", model.ProjectName);
            result.AppendFormatIfNotEmpty(" --system-under-test-version={0}", model.ProjectVersion);

            if (model.IncludeTestResults)
            {
                result.AppendFormatIfNotEmpty(" --link-results-file=\"{0}\"", model.TestResultsFile);

                if (model.TestResultsFormat != TestResultsFormat.NUnit)
                {
                    result.AppendFormat(" --test-results-format={0}", model.TestResultsFormat.ToString().ToLowerInvariant());
                }
            }

            if (!string.Equals(selectedLanguage, "en", StringComparison.OrdinalIgnoreCase))
            {
                result.AppendFormatIfNotEmpty(" --language={0}", selectedLanguage);
            }

            if (documentationFormat != DocumentationFormat.Html)
            {
                result.AppendFormat(" --documentation-format={0}", documentationFormat.ToString().ToLowerInvariant());
            }

            if (model.IncludeExperimentalFeatures)
            {
                result.Append(" --include-experimental-features");
            }

            if (!model.EnableComments)
            {
                result.Append(" --enableComments=false");
            }

            result.AppendFormatIfNotEmpty(" --excludeTags={0}", model.ExcludeTags);
            result.AppendFormatIfNotEmpty(" --hideTags={0}", model.HideTags);

            return result.ToString();
        }

    }
}