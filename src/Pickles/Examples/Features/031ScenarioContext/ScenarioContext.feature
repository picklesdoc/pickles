Feature: Scenario Context features
	In order to show how to use ScenarioContext  
	As a SpecFlow evangelist  
	I want to write some simple scenarios with data in ScenarioContext

Scenario: Store and retrive Person Marcus from ScenarioContext
	When I store a person called Marcus in the Current ScenarioContext
	Then a person called Marcus can easily be retrieved

@showUpInScenarioInfo @andThisToo
Scenario: Showing information of the scenario
	When I execute any scenario
	Then the ScenarioInfo contains the following information
		| Field | Value                               |
		| Tags  | showUpInScenarioInfo, andThisToo    |
		| Title | Showing information of the scenario |

Scenario: Show the type of step we're currently on
	Given I have a Given step
		And I have another Given step
	When I have a When step
	Then I have a Then step

#This is not so easy to write a scenario for but I've created an AfterScenario-hook
#To see this in action remove the @ignore tag below
@ignore @showingErrorHandling 
Scenario: Display error information in AfterScenario
	When an error occurs in a step

Scenario: Pending step
	When I set the ScenarioContext.Current to pending
	Then this step will not even be executed