Feature: Feature Description
    In order include the description (In order, As a, I want) for each feature
    As a reader of the generated documentation
    I want the feature description included below the feature heading.

Scenario: Output has feature description for feature
    Given I have a feature called 'My Described Feature'

    And I have the description
        | Description                                                                   |
        | In order to include the description (In order, As a, I want) for each feature |
        | As a reader of the generated documentation                                    |
        | I want the feature description included below the feature heading.            |

    When I generate Markdown output

    Then the Markdown output has the lines in the following order
        | Content                                                                       |
        | ### My Described Feature                                                      |
        | In order to include the description (In order, As a, I want) for each feature |
        | As a reader of the generated documentation                                    |
        | I want the feature description included below the feature heading.            |