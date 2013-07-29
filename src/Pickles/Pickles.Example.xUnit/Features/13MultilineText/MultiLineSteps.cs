using System;
using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.Example.xUnit.Features._13MultilineText
{
    [Binding]
    public class MultiLineSteps
    {
        private string multiLineText;

        [Given(@"I have read in some text from the user")]
        public void GivenIHaveReadInSomeTextFromTheUser(string multilineText)
        {
            multiLineText = multilineText;
        }

        [When(@"I process this input")]
        public void WhenIProcessThisInput()
        {
            Console.WriteLine("TESTING: Processing '{0}'", multiLineText);
        }

        [Then(@"the result will be saved to the multiline text data store")]
        public void ThenTheResultWillBeSavedToTheMultilineTextDataStore()
        {
            Console.WriteLine("TESTING: Saving: {0}", multiLineText);
        }
    }
}
