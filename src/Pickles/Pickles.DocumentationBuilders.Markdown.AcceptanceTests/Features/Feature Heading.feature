Feature: Feature Heading
    In order to identify each feature file in the documentation
    As a reader of the generated documentation
    I want the feature title visually differentiated from the other text.

Scenario: Output has feature section for each feature

    Given I have a feature called 'My First Feature'
    And I have a feature called 'My Second Feature'

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content               |
        | ### My First Feature  |
        | ### My Second Feature |