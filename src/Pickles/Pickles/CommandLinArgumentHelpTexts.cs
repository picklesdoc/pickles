//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CommandLinArgumentHelpTexts.cs" company="PicklesDoc">
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
namespace PicklesDoc.Pickles
{
    public static class CommandLinArgumentHelpTexts
    {
        public const string HelpFeatureDir = "directory to start scanning recursively for features";
        public const string HelpOutputDir = "directory where output files will be placed";
        public const string HelpSutName = "the name of the system under test";
        public const string HelpSutVersion = "the version of the system under test";
        public const string HelpLanguageFeatureFiles = "the language of the feature files";
        public const string HelpDocumentationFormat = "the format of the output documentation";
        public const string HelpTestResultsFormat = "the format of the linked test results (nunit|nunit3|xunit|xunit2|mstest |cucumberjson|specrun|vstest)";
        public const string HelpIncludeExperimentalFeatures = "whether to include experimental features";
        public const string HelpEnableComments = "whether to enable comments in the output";
        public const string HelpExcludeTags = "exclude scenarios that match this tag";
        public const string HelpHideTags = "Technical tags that shouldn't be displayed (separated by ;)";

        public const string HelpTestResultsFile =
            "the path to the linked test results file (can be a semicolon-separated list of files)";

    }
}