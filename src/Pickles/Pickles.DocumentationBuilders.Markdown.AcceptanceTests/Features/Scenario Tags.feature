Feature: Scenario Tags
    In order to include the tags for each scenario
    As a reader of the generated documentation
    I want the scenario tags included in single line above the scenario heading.

Scenario: Output has tags for scenario
    Given I have a feature called 'My Tagged Feature'
    And I have a scenario called 'Scenario With Tags'
    And I have the scenario tags
        | Tag     |
        | @ignore |
        | @tagtwo |

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content                           |
        | *`@ignore`* *`@tagtwo`*           |
        | #### Scenario: Scenario With Tags |