Feature: File Location
    In order to control where the document is created
    As a document reader
    I want a to be able to specify a folder for the file


Scenario: Output is written to specified location

    Given I specify the output folder as 'C:\testing'
    
    When I generate Markdown output
    
    Then the file 'C:\testing\features.md' exists