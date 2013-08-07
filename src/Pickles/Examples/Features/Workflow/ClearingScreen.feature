Feature: Clearing Screen
	In order to restart a new set of calculations
	As a math idiot
	I want to be able to clear the screen

	@workflow @slow
	Scenario: Clear the screen
		Given I have entered 50 into the calculator
		And I have entered 70 into the calculator
		When I press C
		Then the screen should be empty
