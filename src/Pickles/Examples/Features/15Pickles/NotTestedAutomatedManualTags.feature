Feature: Kinds of verification
	In order to increase stakeholder engagement with pickled specs
	As a SpecFlow evangelist
	I want to make clear how many scenarios are automated, manually tested and not tested
	So that the stakeholders have a better overview of progress

@automated
Scenario: Verified automatically
	Given a feature that is verified automatically
	When I run pickles
	Then it appears in the @automated summary

@manual
Scenario: Verified manually
	Given a feature that is verified manually
	When I run pickles
	Then it appears in the @manual summary

@notTested_waitingForAutomation
Scenario: Verified automatically
	Given a feature that is not verified
	When I run pickles
	Then it appears in the @notTested summary
