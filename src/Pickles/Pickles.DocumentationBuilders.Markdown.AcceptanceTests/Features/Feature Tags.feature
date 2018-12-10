Feature: Feature Tags
    In order to include the tags for each feature
    As a reader of the generated documentation
    I want the feature tags included in single line above the feature heading.

Scenario: Output has tags for feature
    Given I have a feature called 'My Tagged Feature'
    And I have the tags
        | Tag     |
        | @ignore |
        | @tagtwo |

    When I generate Markdown output

    Then the Markdown output has the lines in the following order
        | Content                 |
        | *`@ignore`* *`@tagtwo`* |
        | ### My Tagged Feature   |