Feature: Step Argument Transformations
	In order to reduce the amount of code and repetitive tasks in my steps  
	As a SpecFlow evanglist  
	I want to define reusable transformations for my step arguments
	
Scenario: Steps with non-string arguments 
	Given Dan has been registered at date 2003/03/13 
		And Aslak has been registered at terminal 2
	Then I should be able to see Aslak at terminal 2