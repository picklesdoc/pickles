using System;
using System.Collections.Generic;
using Shouldly;
using TechTalk.SpecFlow;

namespace AutomationLayer
{
    [Binding]
    public class AdditionSteps
    {
        private List<int> numbersList;

        private int result;

        [Given(@"the calculator has clean memory")]
        public void GivenTheCalculatorHasCleanMemory()
        {
            this.numbersList = new List<int>();
            this.result = 0;
        }

        [Given(@"the background step fails")]
        public void GivenTheBackgroundStepFails()
        {
            1.ShouldBe(2);
        }

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(Decimal p0)
        {
            this.numbersList.Add((int)p0);
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            foreach (var i in this.numbersList)
            {
                this.result += i;
            }
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            this.result.ShouldBe(p0);
        }
    }
}
