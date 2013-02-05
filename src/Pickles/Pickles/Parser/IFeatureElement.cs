using System.Collections.Generic;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.Parser
{
    public interface IFeatureElement
    {
        string Description { get; set; }
        Feature Feature { get; set; }
        string Name { get; set; }
        List<Step> Steps { get; set; }
        List<string> Tags { get; set; }
        TestResult Result { get; set; }
    }
}