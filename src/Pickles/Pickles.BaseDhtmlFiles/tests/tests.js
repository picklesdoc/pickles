test("Can convert camel cased string to spaced string", function () {
    equal(addSpacesToCamelCasedString("ThisIsATestString"), "This Is A Test String", "Happy path test");
    equal(addSpacesToCamelCasedString("00ThisIsATestString"), "00 This Is A Test String", "String prefaced by numbers");
    equal(addSpacesToCamelCasedString("ThisIsATestString."), "This Is A Test String.", "String ending with puncuation.");
});

test("Can pull of # from hashtag", function() {
    equal(removeBeginningHash("#hash"), "hash");
    equal(removeBeginningHash("hash"), "hash");
    equal(removeBeginningHash(""), "");
});

var sampleJSONForHeirarchy = [
    {
        "RelativeFolder": "ClearingScreen.feature",
        "Feature": {
            "Name": "Clearing Screen",
            "Description": "In order to restart a new set of calculations\r\nAs a math idiot\r\nI want to be able to clear the screen",
            "FeatureElements": [
            ],
            "Tags": []
        }
    },
    {
        "RelativeFolder": "12NestedFolders\\ChildFolder\\ChildChildFolder\\NestedFolderExample.feature",
        "Feature": {
            "Name": "Nested Folder Example 1",
            "Description": "In order to test nested folder output\r\nAs a silly contributer\r\nI want to create an example of something several folders deep",
            "FeatureElements": [
            ],
            "Tags": []
        }
    },
    {
        "RelativeFolder": "12NestedFolders\\ChildFolder\\NestedFolderExample2.feature",
        "Feature": {
            "Name": "Nested Folder Example 2",
            "Description": "In order to test nested folder output\r\nAs a silly contributer\r\nI want to create an example of something several folders deep",
            "FeatureElements": [
            ],
            "Tags": []
        }
    }
];

test("Can get folder heirarchy from JSON", function () {

    ok(sampleJSONForHeirarchy != null, "Sample JSON object isn't null");
    equal(sampleJSONForHeirarchy.length, 3, "Sample JSON has 3 items");
    {
        var expectList = new Array();
        expectList.push(new Feature('Clearing Screen', 'ClearingScreen.feature'));
        expectList.push(new Feature('Nested Folder Example 1', '12NestedFolders\\ChildFolder\\ChildChildFolder\\NestedFolderExample.feature'));
        expectList.push(new Feature('Nested Folder Example 2', '12NestedFolders\\ChildFolder\\NestedFolderExample2.feature'));
        deepEqual(
            getFeaturesFromScenariosList(sampleJSONForHeirarchy),
            expectList,
            "Get Features From Scenarios List - Happy path");
    }
    {
        // Split Directory Path Into Array Of Folders
        deepEqual(
            splitDirectoryPathIntoArrayOfFormattedFolders('2NestedFolders\\ChildFolder\\NestedFolderExample.feature'),
            ['2 Nested Folders', 'Child Folder', 'NestedFolderExample.feature'],
            'Split Directory Path Into Array Of Folders'
        );
    }
    {
        var startingPaths = new Array();
        startingPaths.push(new Feature('Root Feature #1', 'RootFeature.feature'));
        startingPaths.push(new Feature('Root Feature #2', 'RootFeature2.feature'));
        var expected = new Directory('');
        expected.features.push(new Feature('Root Feature #1', 'RootFeature.feature'));
        expected.features.push(new Feature('Root Feature #2', 'RootFeature2.feature'));
        deepEqual(buildFullHierarchy(startingPaths), expected, "Convert array of paths to Folders / files - two features off the root.");
    }
    {
        var startingPaths = new Array();
        startingPaths.push(new Feature('Nested Folder Example #1', '12NestedFolders\\NestedFolderExample1.feature'));
        startingPaths.push(new Feature('Nested Folder Example #2', '12NestedFolders\\NestedFolderExample2.feature'));
        var expected = new Directory('');
        expected.SubDirectories.push(new Directory('12 Nested Folders'));
        expected.SubDirectories[0].features.push(new Feature('Nested Folder Example #1', '12NestedFolders\\NestedFolderExample1.feature'));
        expected.SubDirectories[0].features.push(new Feature('Nested Folder Example #2', '12NestedFolders\\NestedFolderExample2.feature'));
        deepEqual(buildFullHierarchy(startingPaths), expected, "Convert array of paths to Folders / files - two features under a folder.");
    }
    {
        // SCENARIO: End Goal!
        var startingPaths = getFeaturesFromScenariosList(sampleJSONForHeirarchy);
        var expectedDirStructure = new Directory("");
        expectedDirStructure.features.push(new Feature('Clearing Screen', 'ClearingScreen.feature'));
        expectedDirStructure.SubDirectories.push(new Directory('12 Nested Folders'));
        expectedDirStructure.SubDirectories[0].SubDirectories.push(new Directory('Child Folder'));
        expectedDirStructure.SubDirectories[0].SubDirectories[0].features.push(new Feature('Nested Folder Example 2', '12NestedFolders\\ChildFolder\\NestedFolderExample2.feature'));
        expectedDirStructure.SubDirectories[0].SubDirectories[0].SubDirectories.push(new Directory('Child Child Folder'));
        expectedDirStructure.SubDirectories[0].SubDirectories[0].SubDirectories[0].features.push(new Feature('Nested Folder Example 1', '12NestedFolders\\ChildFolder\\ChildChildFolder\\NestedFolderExample.feature'));
        deepEqual(buildFullHierarchy(startingPaths), expectedDirStructure, 'End Goal!');
    }
});

var sampleJSONForTypeAhead = [
    {
        "RelativeFolder": "Workflow\\ClearingScreen.feature",
        "Feature": {
            "Name": "Clearing Screen",
            "Description": "In order to restart a new set of calculations\r\nAs a math idiot\r\nI want to be able to clear the screen",
            "FeatureElements": [
                {
                    "Name": "Clear the screen",
                    "Description": "",
                    "Steps": [
                        {
                            "Keyword": "Given",
                            "NativeKeyword": "Given ",
                            "Name": "I have entered 50 into the calculator"
                        },
                        {
                            "Keyword": "And",
                            "NativeKeyword": "And ",
                            "Name": "I have entered 70 into the calculator"
                        },
                        {
                            "Keyword": "When",
                            "NativeKeyword": "When ",
                            "Name": "I press C"
                        },
                        {
                            "Keyword": "Then",
                            "NativeKeyword": "Then ",
                            "Name": "the screen should be empty"
                        }
                    ],
                    "Tags": [
                        "@workflow",
                        "@slow"
                    ]
                }
            ],
            "Tags": [
                "@clearing"
            ]
        },
        "Result": {
            "WasExecuted": false,
            "WasSuccessful": false
        }
    },
    {
        "RelativeFolder": "12NestedFolders\\ChildFolder\\ChildChildFolder\\NestedFolderExample.feature",
        "Feature": {
            "Name": "Nested Folder Example",
            "Description": "In order to test nested folder output\r\nAs a silly contributer\r\nI want to create an example of something several folders deep",
            "FeatureElements": [
                {
                    "Name": "Nested - Add two numbers",
                    "Description": "",
                    "Steps": [
                        {
                            "Keyword": "Given",
                            "NativeKeyword": "Given ",
                            "Name": "I have entered 50 into the calculator"
                        },
                        {
                            "Keyword": "And",
                            "NativeKeyword": "And ",
                            "Name": "I have entered 70 into the calculator"
                        },
                        {
                            "Keyword": "When",
                            "NativeKeyword": "When ",
                            "Name": "I press add"
                        },
                        {
                            "Keyword": "Then",
                            "NativeKeyword": "Then ",
                            "Name": "the result should be 120 on the screen"
                        }
                    ],
                    "Tags": [
                        "@nestedFolders"
                    ]
                }
            ],
            "Tags": []
        },
        "Result": {
            "WasExecuted": false,
            "WasSuccessful": false
        }
    }
];

var sampleJSONForTypeAheadWithDuplicateTagsAndScenarioNames = [
    {
        "RelativeFolder": "Workflow\\ClearingScreen.feature",
        "Feature": {
            "Name": "Clearing Screen",
            "Description": "In order to restart a new set of calculations\r\nAs a math idiot\r\nI want to be able to clear the screen",
            "FeatureElements": [
                {
                    "Name": "Clear the screen",
                    "Description": "",
                    "Steps": [
                        {
                            "Keyword": "Given",
                            "NativeKeyword": "Given ",
                            "Name": "I have entered 50 into the calculator"
                        }
                    ],
                    "Tags": [
                        "@workflow",
                        "@slow"
                    ]
                },
                {
                    "Name": "Clear the screen",
                    "Description": "",
                    "Steps": [
                        {
                            "Keyword": "Given",
                            "NativeKeyword": "Given ",
                            "Name": "I have entered 50 into the calculator"
                        }
                    ],
                    "Tags": [
                        "@workflow",
                        "@slow"
                    ]
                }
            ],
            "Tags": []
        },
        "Result": {
            "WasExecuted": false,
            "WasSuccessful": false
        }
    }
];

test("Can build a list of tags and scenario titles for type ahead box", function () {
    {
        var expectedTags = ["@clearing", "@nestedFolders", "@slow", "@workflow"];
        deepEqual(getFeatureAndScenarioTags(sampleJSONForTypeAhead), expectedTags,
            "Will collect tags from all scenarios and features.");
    }
    {
        var expectedNames = ['Clear the screen', 'Clearing Screen', 'Nested - Add two numbers', 'Nested Folder Example'];
        deepEqual(getFeatureAndScenarioNames(sampleJSONForTypeAhead), expectedNames,
            "Will collect all scenario and feature names.");
    }
    {
        var expectedCombinedList = ['@clearing', '@nestedFolders', '@slow', '@workflow', 'Clear the screen', 'Clearing Screen', 'Nested - Add two numbers', 'Nested Folder Example'];
        deepEqual(getTagsAndFeatureAndScenarioNames(sampleJSONForTypeAhead), expectedCombinedList,
            "Will get a combined list of tags and features, sorted.");
    }
    {
        var expectedCombinedListWithNoDupes = ['@slow', '@workflow', 'Clear the screen', 'Clearing Screen'];
        deepEqual(getTagsAndFeatureAndScenarioNames(sampleJSONForTypeAheadWithDuplicateTagsAndScenarioNames), expectedCombinedListWithNoDupes,
            "Will get a combined list of tags and features, sorted, and with no duplicate.");
    }
});

var sampleJSONForSearch = [
    {
        "RelativeFolder": "Workflow\\ClearingScreen.feature",
        "Feature": {
            "Name": "Clearing Screen - Daily",
            "Description": "In order to restart a new set of calculations\r\nAs a math idiot\r\nI want to be able to clear the screen",
            "FeatureElements": [
                {
                    "Name": "Clear the screen",
                    "Description": "",
                    "Steps": [
                        {
                            "Keyword": "Given",
                            "NativeKeyword": "Given ",
                            "Name": "I have entered 50 into the calculator"
                        },
                        {
                            "Keyword": "And",
                            "NativeKeyword": "And ",
                            "Name": "I have entered 70 into the calculator"
                        },
                        {
                            "Keyword": "When",
                            "NativeKeyword": "When ",
                            "Name": "I press C"
                        },
                        {
                            "Keyword": "Then",
                            "NativeKeyword": "Then ",
                            "Name": "the screen should be empty"
                        }
                    ],
                    "Tags": [
                        "@workflow",
                        "@slow"
                    ]
                }
            ],
            "Tags": [
                "@clearing"
            ]
        },
        "Result": {
            "WasExecuted": false,
            "WasSuccessful": false
        }
    },
    {
        "RelativeFolder": "12NestedFolders\\ChildFolder\\ChildChildFolder\\NestedFolderExample.feature",
        "Feature": {
            "Name": "Nested Folder Example",
            "Description": "In order to test nested folder output\r\nAs a silly contributer\r\nI want to create an example of something several folders deep",
            "FeatureElements": [
                {
                    "Name": "Nested - Add two numbers - Daily",
                    "Description": "",
                    "Steps": [
                        {
                            "Keyword": "Given",
                            "NativeKeyword": "Given ",
                            "Name": "I have entered 50 into the calculator"
                        },
                        {
                            "Keyword": "And",
                            "NativeKeyword": "And ",
                            "Name": "I have entered 70 into the calculator"
                        },
                        {
                            "Keyword": "When",
                            "NativeKeyword": "When ",
                            "Name": "I press add"
                        },
                        {
                            "Keyword": "Then",
                            "NativeKeyword": "Then ",
                            "Name": "the result should be 120 on the screen"
                        }
                    ],
                    "Tags": [
                        "@nestedFolders",
                        "@slow"
                    ]
                }
            ],
            "Tags": []
        },
        "Result": {
            "WasExecuted": false,
            "WasSuccessful": false
        }
    }
];

test("Can search for tags and feature/scenarios names", function () {

    deepEqual(getFeaturesMatching('Clearing Screen', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Feature Name search");
    deepEqual(getFeaturesMatching('clearing screen', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Feature Name search - case insensitive");
    deepEqual(getFeaturesMatching('clearing', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Feature Name partial search - case insensitive");
    deepEqual(getFeaturesMatching('Clear the screen', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Scenario Name search");
    deepEqual(getFeaturesMatching('clear THE Screen', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Scenario Name search - case insensitive");
    deepEqual(getFeaturesMatching('the', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Scenario Name partial search - case insensitive");
    deepEqual(getFeaturesMatching('Daily', sampleJSONForSearch), sampleJSONForSearch,
        "Scenario & Feature Name partial search - across multiple features");
    deepEqual(getFeaturesMatching('@clearing', sampleJSONForSearch), [sampleJSONForSearch[0]],
        "Scenario level tag search");
    deepEqual(getFeaturesMatching('@nestedFolders', sampleJSONForSearch), [sampleJSONForSearch[1]],
        "Feature level level tag search");
    deepEqual(getFeaturesMatching('@slow', sampleJSONForSearch), sampleJSONForSearch,
        "Feature level level tag search - across multiple features");
    deepEqual(getFeaturesMatching('@doesnotexist', sampleJSONForSearch), [],
        "Neither a tag or feature in list.");
});

test("Can find feature by RelativeFolder", function() {
    deepEqual(findFeatureByRelativeFolder('Workflow\\ClearingScreen.feature', sampleJSONForSearch), sampleJSONForSearch[0],
        "Feature exists");
    deepEqual(findFeatureByRelativeFolder('Workflow\\ClearingScreen2.feature', sampleJSONForSearch), null,
        "Feature does not exist");
});





