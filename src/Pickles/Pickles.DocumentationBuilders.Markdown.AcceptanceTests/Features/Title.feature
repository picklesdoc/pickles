Feature: Title
    In order to easily tell that this file is a Gherkin output file
    As a document reader
    I want a title in the document as the first line

Scenario: Output has title on first line

    When I generate Markdown output

    Then the Markdown output has the line
        | Line No. | Content    |
        | 1        | # Features |
