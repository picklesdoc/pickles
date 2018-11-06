Feature: Scenario Heading
    In order identify each scenario
    As a reader of the generated documentation
    I want the heading included in the output.


Scenario: Output has scenario heading

    Given I have a feature called 'My Scenario Heading'
    And I have a scenario called 'Scenario heading included in output'

    When I generate Markdown output

    Then the Markdown output has the lines
        | Content                                            |
        | #### Scenario: Scenario heading included in output |


Scenario: Output has scenario heading for each scenario in different features

    Given I have a feature called 'My Scenario Heading A'
    And I have a scenario called 'Scenario heading included in output - A'

    And I have a feature called 'My Scenario Heading B'
    And I have a scenario called 'Scenario heading included in output - B'

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content                                                |
        | ### My Scenario Heading A                              |
        | #### Scenario: Scenario heading included in output - A |
        | ### My Scenario Heading B                              |
        | #### Scenario: Scenario heading included in output - B |


Scenario: Output has scenario heading for each scenario in same feature

    Given I have a feature called 'My Scenario Heading'
    And I have a scenario called 'Scenario heading included in output - 1'
    And I have a scenario called 'Scenario heading included in output - 2'

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content                                                |
        | ### My Scenario Heading                                |
        | #### Scenario: Scenario heading included in output - 1 |
        | #### Scenario: Scenario heading included in output - 2 |