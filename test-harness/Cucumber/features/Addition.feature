Feature: Addition
  In order to avoid silly mistakes
  As a math idiot
  I want to be told the sum of two numbers

Background:
  Given the calculator has clean memory

@tag2
Scenario Outline: Adding several numbers
  Given I have entered <first number> into the calculator
  And I have entered <second number> into the calculator
  And I have entered <third number> into the calculator
  When I press add
  Then the result should be <result> on the screen

Examples:
  | first number | second number | third number | result |
  | 60           | 70            | 130          | 260    |
  | 40           | 50            | 90           | 180    |

@tag1
Scenario: Add two numbers
  Given I have entered 1 into the calculator
  And I have entered 2 into the calculator
  When I press add
  Then the result should be 3 on the screen

@tag1
Scenario: Fail to add two numbers
  Given I have entered 1 into the calculator
  And I have entered 2.2 into the calculator
  When I press add
  Then the result should be 3.2 on the screen

@ignore
Scenario: Ignored adding two numbers
  Given I have entered 1 into the calculator
  And I have entered 2.2 into the calculator
  When I press add
  Then the result should be 3.2 on the screen

Scenario: Not automated adding two numbers
  Given unimplemented step
  When unimplemented step
  Then unimplemented step
