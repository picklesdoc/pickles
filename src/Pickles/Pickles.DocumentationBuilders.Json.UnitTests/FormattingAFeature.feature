Feature: Formatting A Feature

@json
Scenario: A simple feature

    Given I have this feature description
        """
        Feature: Clearing Screen
            In order to restart a new set of calculations
            As a math idiot
            I want to be able to clear the screen

        @workflow @slow
        Scenario: Clear the screen
            Given I have entered 50 into the calculator
            And I have entered 70 into the calculator
            When I press C
            Then the screen should be empty
        """
    When I generate the documentation
    Then the JSON file should contain
        """
        {
          "Features": [
            {
              "RelativeFolder": "",
              "Feature": {
                "Name": "Clearing Screen",
                "Description": "In order to restart a new set of calculations\r\nAs a math idiot\r\nI want to be able to clear the screen",
                "FeatureElements": [
                  {
                    "Name": "Clear the screen",
                    "Slug": "clear-the-screen",
                    "Description": "",
                    "Steps": [
                      {
                        "Keyword": "Given",
                        "NativeKeyword": "Given ",
                        "Name": "I have entered 50 into the calculator",
                        "StepComments": [],
                        "AfterLastStepComments": []
                      },
                      {
                        "Keyword": "And",
                        "NativeKeyword": "And ",
                        "Name": "I have entered 70 into the calculator",
                        "StepComments": [],
                        "AfterLastStepComments": []
                      },
                      {
                        "Keyword": "When",
                        "NativeKeyword": "When ",
                        "Name": "I press C",
                        "StepComments": [],
                        "AfterLastStepComments": []
                      },
                      {
                        "Keyword": "Then",
                        "NativeKeyword": "Then ",
                        "Name": "the screen should be empty",
                        "StepComments": [],
                        "AfterLastStepComments": []
                      }
                    ],
                    "Tags": [
                      "@workflow",
                      "@slow"
                    ],
                    "Result": {
                      "WasExecuted": false,
                      "WasSuccessful": false
                    }
                  }
                ],
                "Result": {
                  "WasExecuted": false,
                  "WasSuccessful": false
                },
                "Tags": []
              },
              "Result": {
                "WasExecuted": false,
                "WasSuccessful": false
              }
            }
          ],
        """

@json
Scenario: A feature with a table

    Given I have this feature description
        """
        Feature: Interactive DHTML View
            In order to increase stakeholder engagement with pickled specs
            As a SpecFlow evangelist
            I want to adjust the level of detail in the DHTML view to suit my audience
            So that I do not overwhelm them.

        Scenario: Scenario with large data table
            Given a feature with a large table of data:
                | heading    | page # |
                | Chapter 1  | 1      |
                | Chapter 2  | 5      |
                | Chapter 3  | 10     |
                | Chapter 4  | 15     |
                | Chapter 5  | 20     |
                | Chapter 6  | 25     |
                | Chapter 7  | 30     |
                | Chapter 8  | 35     |
                | Chapter 9  | 40     |
                | Chapter 10 | 45     |
                | Chapter 11 | 50     |
                | Chapter 12 | 55     |
                | Chapter 13 | 60     |
                | Chapter 14 | 65     |
                | Chapter 15 | 70     |
                | Chapter 16 | 75     |
                | Chapter 17 | 80     |
                | Chapter 18 | 85     |
                | Chapter 19 | 90     |
                | Chapter 20 | 95     |
                | Chapter 21 | 100    |
                | Chapter 22 | 105    |
            When I click on the table heading
            Then the table body should collapse
        """
    When I generate the documentation
    Then the JSON file should contain
        """
        {
          "Features": [
            {
              "RelativeFolder": "",
              "Feature": {
                "Name": "Interactive DHTML View",
                "Description": "In order to increase stakeholder engagement with pickled specs\r\nAs a SpecFlow evangelist\r\nI want to adjust the level of detail in the DHTML view to suit my audience\r\nSo that I do not overwhelm them.",
                "FeatureElements": [
                  {
                    "Name": "Scenario with large data table",
                    "Slug": "scenario-with-large-data-table",
                    "Description": "",
                    "Steps": [
                      {
                        "Keyword": "Given",
                        "NativeKeyword": "Given ",
                        "Name": "a feature with a large table of data:",
                        "TableArgument": {
                          "HeaderRow": [
                            "heading",
                            "page #"
                          ],
                          "DataRows": [
                            [
                              "Chapter 1",
                              "1"
                            ],
                            [
                              "Chapter 2",
                              "5"
                            ],
                            [
                              "Chapter 3",
                              "10"
                            ],
                            [
                              "Chapter 4",
                              "15"
                            ],
                            [
                              "Chapter 5",
                              "20"
                            ],
                            [
                              "Chapter 6",
                              "25"
                            ],
                            [
                              "Chapter 7",
                              "30"
                            ],
                            [
                              "Chapter 8",
                              "35"
                            ],
                            [
                              "Chapter 9",
                              "40"
                            ],
                            [
                              "Chapter 10",
                              "45"
                            ],
                            [
                              "Chapter 11",
                              "50"
                            ],
                            [
                              "Chapter 12",
                              "55"
                            ],
                            [
                              "Chapter 13",
                              "60"
                            ],
                            [
                              "Chapter 14",
                              "65"
                            ],
                            [
                              "Chapter 15",
                              "70"
                            ],
                            [
                              "Chapter 16",
                              "75"
                            ],
                            [
                              "Chapter 17",
                              "80"
                            ],
                            [
                              "Chapter 18",
                              "85"
                            ],
                            [
                              "Chapter 19",
                              "90"
                            ],
                            [
                              "Chapter 20",
                              "95"
                            ],
                            [
                              "Chapter 21",
                              "100"
                            ],
                            [
                              "Chapter 22",
                              "105"
                            ]
                          ]
                        },
                        "StepComments": [],
                        "AfterLastStepComments": []
                      },
                      {
                        "Keyword": "When",
                        "NativeKeyword": "When ",
                        "Name": "I click on the table heading",
                        "StepComments": [],
                        "AfterLastStepComments": []
                      },
                      {
                        "Keyword": "Then",
                        "NativeKeyword": "Then ",
                        "Name": "the table body should collapse",
                        "StepComments": [],
                        "AfterLastStepComments": []
                      }
                    ],
                    "Tags": [],
                    "Result": {
                      "WasExecuted": false,
                      "WasSuccessful": false
                    }
                  }
                ],
                "Result": {
                  "WasExecuted": false,
                  "WasSuccessful": false
                },
                "Tags": []
              },
              "Result": {
                "WasExecuted": false,
                "WasSuccessful": false
              }
            }
          ],
        """