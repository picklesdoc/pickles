Feature: Scenario Steps
    In order to document the Gherkin steps
    As a reader of the generated documentation
    I want the scenario steps to appear in order below the scenario heading.

Scenario: Output has simple given step for scenario
    Given I have a feature called 'My Scenario Steps Feature'
    And I have a scenario called 'Simple Given Step'
    And I have the scenario steps
        | Keyword | Step                       |
        | Given   | I have a simple given step |

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content                                |
        | #### Scenario: Simple Given Step       |
        | >                                      |
        | > **Given** I have a simple given step |


Scenario: Output has simple given & and step for scenario
    Given I have a feature called 'My Scenario Steps Feature'
    And I have a scenario called 'Multiple Given Steps'
    And I have the scenario steps
        | Keyword | Step                       |
        | Given   | I have a simple given step |
        | And     | I have second given        |

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content                                |
        | #### Scenario: Multiple Given Steps    |
        | >                                      |
        | > **Given** I have a simple given step |
        | >                                      |
        | > **And** I have second given          |


Scenario: Output has a step with a table for scenario
    Given I have a feature called 'My Scenario Steps Feature'
    And I have a scenario called 'Table Given Step'
    And I have the scenario step with table 'Given I have a table'
        | TableColA | TableColb |
        | TDA1      | TDB1      |
        | TDA2      | TDB2      |

    When I generate Markdown output
    
    Then the Markdown output has the lines in the following order
        | Content                         |
        | #### Scenario: Table Given Step |
        | >                               |
        | > **Given** I have a table      |
        | >                               |
        | > \| TableColA \| TableColb \|  |
        | > \| --- \| --- \|              |
        | > \| TDA1 \| TDB1 \|            |
        | > \| TDA2 \| TDB2 \|            |