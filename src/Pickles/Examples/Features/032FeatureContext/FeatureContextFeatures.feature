@showUpInScenarioInfo @andThisToo
Feature: FeatureContext features
	In order to show how to use FeatureContext
	As a SpecFlow evangelist
	I want to write some simple scenarios with data in FeatureContext
	
Scenario: Store and retrive Person Marcus from FeatureContext Current
	When I store a person called Marcus in the current FeatureContext
	Then a person called Marcus can easily be retrieved from the current FeatureContext

Scenario: Showing information of the feature
	When I execute any scenario in the feature
	Then the FeatureInfo contains the following information
		| Field          | Value                               |
		| Tags           | showUpInScenarioInfo, andThisToo    |
		| Title          | FeatureContext features             |
		| TargetLanguage | CSharp                              |
		| Language       | en-US                               |
		| Description    | In order to                         |