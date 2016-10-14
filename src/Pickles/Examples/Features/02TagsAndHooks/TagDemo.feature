@allAboutTags @important
Feature: Tag demonstrator
	In order to show the capabilities of tags in SpecFlow  
	As a SpecFlow evanglist  
	I want to write scenarios that has tags and show their usage in code
	
@ignore
Scenario: Ignored scenario
	Given that my scenario has the @ignore tag
	When I run the scenario
	Then the scenario is ignored
		And the missing step definitions are not reported 

Scenario: A scenario without tags
	Given that my scenario has 0 tags
	When I run the scenario
	Then before scenario hook with '' is run
	
@testTag1
Scenario: A scenario with 1 tag
	Given that my scenario has 1 tags
	When I run the scenario
	Then before scenario hook with 'testTag1' is run

@testTag1 @testTag2 @testTag3
Scenario: A scenario with 3 tags
	Given that my scenario has 3 tags
	When I run the scenario
	Then before scenario hook with 'testTag1, testTag2, testTag3' is run

@testTag1 @testTag3
Scenario: A scenario with 2 tags
	Given that my scenario has 2 tags
	When I run the scenario
	Then before scenario hook with 'testTag1, testTag3' is run