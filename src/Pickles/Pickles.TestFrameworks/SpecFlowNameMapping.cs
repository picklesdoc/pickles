using TechTalk.SpecFlow.Tracing;

namespace PicklesDoc.Pickles.TestFrameworks
{
	internal static class SpecFlowNameMapping
	{
	    public static string Build(string name)
	    {
	        return name.ToIdentifier();
        }
	}
}
