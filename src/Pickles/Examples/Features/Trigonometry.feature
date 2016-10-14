Feature: Trigonometry
	In order to avoid perform more advanced calculations  
	As a math idiot  
	I want to be able to use trigonometric functions

	@trigonometric @fast
	Scenario: Sine
		Given I have entered 90 into the calculator
		When I press sin
		Then the result should be 1 on the screen

	@trigonometric @fast
	Scenario: Cosine
		Given I have entered 0 into the calculator
		When I press cos
		Then the result should be 1 on the screen

	@trigonometric @fast
	Scenario: Tangent
		Given I have entered 45 into the calculator
		When I press tan
		Then the result should be 1 on the screen
