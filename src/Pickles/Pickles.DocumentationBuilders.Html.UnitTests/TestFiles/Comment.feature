Feature: Test
	In order to do something
	As a user
	I want to run this scenario

	Scenario: A scenario
		# A single line comment
		Given some feature
		# A multiline comment - first line
		# second line
		And another feature
		#
		# Multiline with empty first line
		# But the last lines are not empty
		And another feature
		# Multiline with empty last line
		# But the first lines are not empty
		#
		When it runs
		# Multiline with first and last last lines not empty
		# 
		# But the middle line is empty
		Then I should see that this thing happens
		And there is no comment here
		# A comment after the last step