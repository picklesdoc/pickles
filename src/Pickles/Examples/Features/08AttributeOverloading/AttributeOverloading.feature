Feature: Attribute overloading
	In order to show that steps can be used with multiple attributes  
	As a SpecFlow Evangelist  
	I want to show that similar attributes can be applied to the same step definition

Scenario: Checking number for evenness
	Given I have this simple step
		And this simple step
		And also this step
	When I do something
	Then I could validate that the number 2 is even
		And that the number 4 is even
		But the number 3 is odd