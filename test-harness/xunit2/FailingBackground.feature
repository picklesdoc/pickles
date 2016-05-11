Feature: Failing Background
This feature has a failing background.

Background:
    Given the background step fails
    And the calculator has clean memory

Scenario: Add two numbers
    Given I have entered 50 into the calculator
    And I have entered 70 into the calculator
    When I press add
    Then the result should be 120 on the screen

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
