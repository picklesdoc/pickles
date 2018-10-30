Feature: Generated Info
    In order to see when the document was created
    As a document reader
    I want a date and timestamp for when the output was generated

Background:

    Given the date is 2018-10-25 and the time is 18:53:00


Scenario: Output has timestamp on third line

    When I generate Markdown output

    Then the Markdown output has the line
        | Line No. | Content                                   |
        | 3        | Generated on: 25 October 2018 at 18:53:00 |
