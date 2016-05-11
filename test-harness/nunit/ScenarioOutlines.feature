Feature: Scenario Outlines
  Here we demonstrate how we deal with scenario outlines

Scenario Outline: This is a scenario outline where all scenarios pass 
  
  This means the entire scenario outline passes.
  
  Then the scenario will '<result>'

  Examples: 
    | result |
    | pass_1 |
    | pass_2 |
    | pass_3 |

    
Scenario Outline: This is a scenario outline where one scenario is inconclusive
  
  This means the entire scenario outline is inconclusive.

  Then the scenario will '<result>'

  Examples: 
    | result         |
    | pass_1         |
    | pass_2         |
    | inconclusive_1 |


Scenario Outline: This is a scenario outline where one scenario fails
  
  This means the entire scenario outline fails.

  Then the scenario will '<result>'

  Examples: 
    | result |
    | pass_1 |
    | pass_2 |
    | fail_1 |


Scenario Outline: And we can go totally bonkers with multiple example sections.

  Then the scenario will '<result>'

  Examples: 
    | result |
    | pass_1 |
    | pass_2 |

  Examples: 
    | result         |
    | inconclusive_1 |
    | inconclusive_2 |

  Examples: 
    | result |
    | fail_1 |
    | fail_2 |


Scenario Outline: Deal correctly with backslashes in the examples

  When I have backslashes in the value, for example a '<file path>'

  Examples:
    | file path |
    | c:\Temp\  |


Scenario Outline: Deal correctly with parenthesis in the examples

  When I have parenthesis in the value, for example an '<overly descriptive field>'

  Examples:
    | overly descriptive field         |
    | This is a description (and more) |
    
Scenario Outline: Deal correctly with overlong example values
  
  When I have a field with value '<value1>'
  And I have a field with value '<value2>'
  Then the scenario will 'pass_1'
  
  Examples:
    | value1                                                  | value2                                                  |
    | Please enter a valid two letter country code (e.g. DE)! | This is just a very very very veery long error message! |