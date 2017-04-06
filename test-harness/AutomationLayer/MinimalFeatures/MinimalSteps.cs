using System;
using TechTalk.SpecFlow;

namespace AutomationLayer.MinimalFeatures
{
    [Binding]
    public class MinimalSteps
    {
        [Then(@"passing step")]
        public void ThenPassingStep()
        {
            MarkTestAs.Passing();
        }

        [Then(@"inconclusive step")]
        public void ThenInconclusiveStep()
        {
            MarkTestAs.Inconclusive();
        }

        [Then(@"failing step")]
        public void ThenFailingStep()
        {
            MarkTestAs.Failing();
        }
    }
}
