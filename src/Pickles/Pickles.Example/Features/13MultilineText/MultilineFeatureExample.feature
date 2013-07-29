Feature: Multiline Feature Example
	In order capture this particular Gherkin feature
	As a Pickles contributer
	I want to demonstrate an example of using multiline text in a Scenario

@mytag
Scenario: Mutliline Output
	Given I have read in some text from the user
	"""
	This is line 1.
	This is line 2!
	This is line 3!!
	"""
	When I process this input
	Then the result will be saved to the multiline text data store
