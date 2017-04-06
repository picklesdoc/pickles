using System;
using NFluent;
using TechTalk.SpecFlow;

namespace AutomationLayer.MinimalFeatures
{
    [Binding]
    public class MinimalSteps
    {
        [Then(@"passing step")]
        public void ThenPassingStep()
        {
        }

        [Then(@"inconclusive step")]
        public void ThenInconclusiveStep()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"failing step")]
        public void ThenFailingStep()
        {
            Check.That(true).IsEqualTo(false);
        }
    }
}
