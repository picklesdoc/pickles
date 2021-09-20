//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IConfiguration.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.IO.Abstractions;

namespace PicklesDoc.Pickles
{
    public interface IConfiguration
    {
        IDirectoryInfo FeatureFolder { get; set; }

        IDirectoryInfo OutputFolder { get; set; }

        DocumentationFormat DocumentationFormat { get; set; }

        string Language { get; set; }

        TestResultsFormat TestResultsFormat { get; set; }

        bool HasTestResults { get; }

        IFileInfo TestResultsFile { get; }

        IEnumerable<IFileInfo> TestResultsFiles { get; }

        string SystemUnderTestName { get; set; }

        string SystemUnderTestVersion { get; set; }

        bool ShouldIncludeExperimentalFeatures { get; }

        string ExcludeTags { get; set; }
        string HideTags { get; set; }
        Uri FeatureBaseUri { get; set; }

        void AddTestResultFile(IFileInfo IFileInfo);

        void AddTestResultFiles(IEnumerable<IFileInfo> IFileInfos);

        void EnableExperimentalFeatures();

        void DisableExperimentalFeatures();

        bool ShouldEnableComments { get; }

        void EnableComments();

        void DisableComments();
    }
}