Feature: Setting uri for aFeature in a folder

@cucumber
Scenario: A simple feature in a folder without base uri set
    Given I have this feature description placed in a folder 'FeatureTest' in a file 'SettingUriForAFeature.feature'
        """
        Feature: Clearing Screen
        Scenario: Clear the screen
            Given I have entered 50 into the calculator
            And I have entered 70 into the calculator
            When I press C
            Then the screen should be empty
        """
    When I generate the documentation
    Then the JSON file should contain
    """
    "uri": "FeatureTest/SettingUriForAFeature.feature"
    """

@cucumber
Scenario: A simple feature in a folder with file base uri

    Given I have this feature description placed in a folder 'FeatureTest' in a file 'SettingUriForAFeature.feature'
    """
    Feature: Clearing Screen
    Scenario: Clear the screen
       Given I have entered 50 into the calculator
       And I have entered 70 into the calculator
       When I press C
       Then the screen should be empty
    """
    And feature base uri is provided from configuration as 'test'
    When I generate the documentation
    Then the JSON file should contain
    """
    "uri": "test/FeatureTest/SettingUriForAFeature.feature"
    """
@cucumber
Scenario: A simple feature in a folder with folder base uri
    Given I have this feature description placed in a folder 'FeatureTest' in a file 'SettingUriForAFeature.feature'
    """
    Feature: Clearing Screen
    Scenario: Clear the screen
       Given I have entered 50 into the calculator
       And I have entered 70 into the calculator
       When I press C
       Then the screen should be empty
    """
    And feature base uri is provided from configuration as 'root/test/'
    When I generate the documentation
    Then the JSON file should contain
    """
    "uri": "root/test/FeatureTest/SettingUriForAFeature.feature"
    """

@cucumber
Scenario: A simple feature in a folder with an absolute base uri
    Given I have this feature description placed in a folder 'FeatureTest' in a file 'SettingUriForAFeature.feature'
    """
    Feature: Clearing Screen
    Scenario: Clear the screen
       Given I have entered 50 into the calculator
       And I have entered 70 into the calculator
       When I press C
       Then the screen should be empty
    """
    And feature base uri is provided from configuration as 'http://root/test/'
    When I generate the documentation
    Then the JSON file should contain
    """
    "uri": "http://root/test/FeatureTest/SettingUriForAFeature.feature"
    """