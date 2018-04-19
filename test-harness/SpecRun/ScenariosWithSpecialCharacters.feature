Feature: Scenarios With Special Characters
  Here we demonstrate usage of special characters in scenario names

Background:
  Given the calculator has clean memory

Scenario: This is a scenario with parentheses, hyphen and comma (10-20, 30-40)
  Given I have entered 50 into the calculator
  And I have entered 70 into the calculator
  When I press add
  Then the result should be 120 on the screen


Scenario Outline: This is a scenario outline with parentheses, hyphen and comma (10-20, 30-40)
  Then the scenario will '<result>'

  Examples: 
    | result |
    | pass_1 |

Scenario Outline: This scenario contains examples with Regex-special characters
  When I have special characters for regexes in the value, for example a '<regex>'
  Then the scenario will 'PASS'
  
  Examples:
    | regex                          |
    | **                             |
    | ++                             |
    | .*                             |
    | []                             |
    | {}                             |
    | ()                             |
    | ^.*(?<foo>BAR)\s[^0-9]{3,4}A+$ |

Scenario Outline: This is a scenario outline with german umlauts äöüß ÄÖÜ
  Then the scenario will '<result>'

  Examples: 
    | result |
    | pass_1 |

#Excluding this test, because & is not properly encoded in the result so that xml-parsing fails
#Scenario Outline: This is a scenario outline with ampersand &
#  Then the scenario will '<result>'
#
#  Examples: 
#    | result |
#    | pass_1 |

Scenario: This is a scenario with danish characters æøå ÆØÅ
  Then passing step

Scenario: This is a scenario with spanish characters ñáéíóú
  Then passing step