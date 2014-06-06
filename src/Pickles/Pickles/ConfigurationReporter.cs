using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace PicklesDoc.Pickles
{
    public class ConfigurationReporter
    {
        public void ReportOn(Configuration configuration, Action<String> writeToLog)
        {
            writeToLog("Generating documentation based on the following parameters");
            writeToLog("----------------------------------------------------------");
            writeToLog(string.Format("Feature Directory         : {0}", configuration.FeatureFolder.FullName));
            writeToLog(string.Format("Output Directory          : {0}", configuration.OutputFolder.FullName));
            writeToLog(string.Format("Project Name              : {0}", configuration.SystemUnderTestName));
            writeToLog(string.Format("Project Version           : {0}", configuration.SystemUnderTestVersion));
            writeToLog(string.Format("Language                  : {0}", configuration.Language));
            writeToLog(string.Format("Incorporate Test Results? : {0}", configuration.HasTestResults ? "Yes" : "No"));

            if (configuration.HasTestResults)
            {
                writeToLog(string.Format("Test Result Format        : {0}", configuration.TestResultsFormat));
                writeToLog(string.Format("Test Result File(s)       : {0}", GetTestResultFileNames(configuration.TestResultsFiles)));
            }
        }

        private static string GetTestResultFileNames(IEnumerable<FileInfoBase> testResultsFiles)
        {
            var fileNames = testResultsFiles.Select(x => x.FullName).ToArray();
            return string.Join(";", fileNames);
        }

    }
}
