Feature: Showing basic gherkin syntax
	In order to see that gherkin is a very simple language  
	As a SpecFlow evangelist  
	I want to show that basic syntax

	![Test Image](test.jpg)

Scenario: Simple GWT
	Given the initial state of the application is Running
	When I ask what the application state is
	Then I should see Running as the answer

Scenario: Using And and But
	Given the initial state of the application is Running
		And I have authorization to ask application state
	When I ask what the application state is
	Then I should see Running as the answer
		And I should see the time of the application
		But the state of the application should not be Stopped
