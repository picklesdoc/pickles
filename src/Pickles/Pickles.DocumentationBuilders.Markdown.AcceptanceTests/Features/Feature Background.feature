Feature: Background Steps
    In order to document the background Gherkin steps run before each scenario
    As a reader of the generated documentation
    I want the background steps to be included in the Markdown output.

Scenario: Output has background given step for feature

    Given I have a feature called 'My Background Steps Feature'
    And I have a background section
    And I have the background steps
        | Keyword | Step                       |
        | Given   | I have a simple given step |

    When I generate Markdown output

    Then the Markdown output has the lines in the following order
        | Content                                |
        | #### Background:                       |
        | >                                      |
        | > **Given** I have a simple given step |
