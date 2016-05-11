using System;

using Shouldly;

using TechTalk.SpecFlow;

namespace AutomationLayer
{
  [Binding]
  public class ScenarioOutlineSteps
  {
    [Then(@"the scenario will '(.*)'")]
    public void ThenTheScenarioWill(string result)
    {
        if (result.ToUpperInvariant().StartsWith("PASS"))
        {
        // nothing to be done
        }
        else if (result.ToUpperInvariant().StartsWith("FAIL"))
        {
        true.ShouldBe(false);
        }
        else
        {
        ScenarioContext.Current.Pending();
        }
    }

    [When(@"I have special characters for regexes in the value, for example a '(.*)'")]
    public void WhenIHaveSpecialCharactersForRegexesInTheValueForExampleARegex(string regex)
    {
        // nothing to be done
    }    

    [When(@"I have backslashes in the value, for example a '(.*)'")]
    public void WhenIHaveBackslashesInTheValueForExampleAFilePath(string filePath)
    {
        // nothing to be done
    }

    [When(@"I have parenthesis in the value, for example an '(.*)'")]
    public void WhenIHaveParenthesisInTheValueForExampleAnOverlyDescriptiveField(string description)
    {
        // nothing to be done - test case requires pass
    }
    
    [When(@"I have a field with value '(.*)'")]
    public void WhenIHaveAFieldWithValue(string value)
    {
        // nothing to be done - test case requires pass
    }
  }
}
