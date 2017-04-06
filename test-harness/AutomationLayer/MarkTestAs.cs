using NFluent;
using TechTalk.SpecFlow;

namespace AutomationLayer
{
    public static class MarkTestAs
    {
        internal static void Inconclusive()
        {
            ScenarioContext.Current.Pending();
        }

        internal static void Failing()
        {
            Check.That(true).IsEqualTo(false);
        }

        internal static void Passing()
        {
            // nothing to be done
        }
    }
}