Feature: Scenario Outlines
    In order to document the Scenario Outlines
    As a reader of the generated documentation
    I want the scenario outlines to appear with steps and examples below the feature heading.

Scenario: Output has simple scenario outline

    Given I have a feature called 'My Scenario Outline Feature'

    And I have a scenario outline called 'Scenario Outline with Single Example'

    And I have the scenario steps
        | Keyword | Step                                      |
        | Given   | I have a simple given step for <data_one> |
        | And     | I have second given for <data_two>        |

    And I have an examples table
        | Scenario | data_one | data_two |
        | First    | one      | wun      |
        | Second   | two      | too      |
        | Third    | three    | tree     |

    When I generate Markdown output

    Then the Markdown output has the lines in the following order
        | Content                                                     |
        | #### Scenario Outline: Scenario Outline with Single Example |
        | >                                                           |
        | > **Given** I have a simple given step for <data_one>       |
        | >                                                           |
        | > **And** I have second given for <data_two>                |
        | >                                                           |
        | > Examples:                                                 |
        | >                                                           |
        | > \| Scenario \| data_one \| data_two \|                    |
        | > \| --- \| --- \| --- \|                                   |
        | > \| First \| one \| wun \|                                 |
        | > \| Second \| two \| too \|                                |
        | > \| Third \| three \| tree \|                              |