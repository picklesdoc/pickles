Feature: File Location
    In order to control where the document is created
    As a document reader
    I want a to be able to specify an output folder for the document


Scenario: Output is written to specified location

    Given I specify the output folder as 'C:\testing'
    
    When I generate Markdown output
    
    Then the file 'C:\testing\features.md' exists