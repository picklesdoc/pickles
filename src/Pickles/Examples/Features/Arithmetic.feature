Feature: Arithmetic
In order to avoid silly mistakes
As a math idiot
I want to be able to perform arithmetic on the calculator

When $a \ne 0$, there are two solutions to $\(ax^2 + bx + c = 0\)$ and they are
$$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$

	@arithmetic @fast
	Scenario: Add two numbers

$50 + 70 = 120$

	# In the DHTML version, the description will be rendered with mathematical formulas if the experimental features are enabled.
		Given I have entered 50 into the calculator
		And I have entered 70 into the calculator
		When I press add
		Then the result should be 120 on the screen

	@arithmetic @fast
	Scenario: Subtract two numbers
		Given I have entered 50 into the calculator
		And I have entered 70 into the calculator
		When I press subtract
		Then the result should be -20 on the screen

	@arithmetic @fast
	Scenario: Multiply two numbers
		Given I have entered 50 into the calculator
		And I have entered 70 into the calculator
		When I press multiply
		Then the result should be 3500 on the screen

	@arithmetic @fast
	Scenario: Divide two numbers
		Given I have entered 50 into the calculator
		And I have entered 2 into the calculator
		When I press divide
		Then the result should be 25 on the screen
