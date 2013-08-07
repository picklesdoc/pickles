Feature: Show the use of background
	In order to show how to use the Background keyword of Gherkin
	As a SpecFlow evanglist
	I want to show that background steps are called before any scenario step

Background:
	Given I have initialized the Sum-variable to 0
	When I add 1 to the Sum-variable

Scenario: Add 1 to the sum
	When I add 1 to the Sum-variable
	Then the total sum should be 2

Scenario: Add 2 to the sum
	When I add 2 to the Sum-variable
	Then the total sum should be 3
