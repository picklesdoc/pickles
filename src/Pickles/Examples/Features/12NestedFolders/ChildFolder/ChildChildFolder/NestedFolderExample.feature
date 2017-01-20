Feature: Nested Folder Example
	In order to test nested folder output  
	As a silly contributer  
	I want to create an example of something several folders deep

@nestedFolders
Scenario: Nested - Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
