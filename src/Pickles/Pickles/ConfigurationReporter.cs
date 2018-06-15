//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConfigurationReporter.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles
{
    public class ConfigurationReporter
    {
        public void ReportOn(IConfiguration configuration, Action<string> writeToLog)
        {
            writeToLog("Generating documentation based on the following parameters");
            writeToLog("----------------------------------------------------------");
            writeToLog($"Feature Directory              : {configuration.FeatureFolder.FullName}");
            writeToLog($"Output Directory               : {configuration.OutputFolder.FullName}");
            writeToLog($"Project Name                   : {configuration.SystemUnderTestName}");
            writeToLog($"Project Version                : {configuration.SystemUnderTestVersion}");
            writeToLog($"Language                       : {configuration.Language}");
            writeToLog($"Incorporate Test Results?      : {(configuration.HasTestResults ? "Yes" : "No")}");
            writeToLog($"Include Experimental Features? : {(configuration.ShouldIncludeExperimentalFeatures ? "Yes" : "No")}");
            writeToLog($"Exclude Tag                    : {configuration.ExcludeTags}");
            writeToLog($"Technical Tag                    : {configuration.HideTags}");

            if (configuration.HasTestResults)
            {
            writeToLog($"Test Result Format             : {configuration.TestResultsFormat}");
                foreach (var testResultsFile in configuration.TestResultsFiles)
                {
            writeToLog($"Test Result File               : {testResultsFile.FullName}");
                }
            }
        }
    }
}
