Feature: Scenario Status Icons
    In order to display the outcome of Scenarios and Scenario Outlines
    As a reader of the generated documentation
    I want the result to indicated as Pass, Fail, or Inconclusive by an appropriate icon.


Scenario: Status icons are written to specified location

    Given I specify the output folder as 'C:\testing'
    
    When I generate Markdown output
    
    Then the file 'C:\testing\pass.png' exists
    And the file 'C:\testing\fail.png' exists
    And the file 'C:\testing\inconclusive.png' exists


Scenario Outline: Scenario displays results icon
    Given I have a feature called 'My Scenario Results Feature'
    And I have a scenario called 'Scenario with result'
    And the scenario test run outcome was <outcome>

    When I generate Markdown output

    Then the Markdown output has the lines in the following order
        | Content                                                            |
        | #### Scenario: ![<outcome_label>](<outcome_image>) Scenario with result |

Examples:
    | outcome_label | outcome      | outcome_image    |
    | Passed        | passed       | pass.png         |
    | Failed        | failed       | fail.png         |
    | Inconclusive  | inconclusive | inconclusive.png |


Scenario: Scenario Outline Example displays results icon
    Given I have a feature called 'My Scenario Outline Results Feature'
    And I have a scenario outline called 'Scenario Outline with result'
    And I have an examples table with results
        | Scenario | data_one | data_two | outcome      |
        | First    | one      | wun      | passed       |
        | Second   | two      | too      | failed       |
        | Third    | three    | tree     | inconclusive |

    When I generate Markdown output

    Then the Markdown output has the lines in the following order
        | Content                                                             |
        | > Examples:                                                         |
        | >                                                                   |
        | > \| Scenario \| data_one \| data_two \| Result \|                  |
        | > \| --- \| --- \| --- \| --- \|                                    |
        | > \| First \| one \| wun \| ![Passed](pass.png) \|                  |
        | > \| Second \| two \| too \| ![Failed](fail.png) \|                 |
        | > \| Third \| three \| tree \| ![Inconclusive](inconclusive.png) \| |