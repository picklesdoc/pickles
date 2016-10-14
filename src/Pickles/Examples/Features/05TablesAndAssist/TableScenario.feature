Feature: Showing table usage
	In order to show how to use tables  
	As a SpecFlow evanglist  
	I want to write some simple scenarios that uses tables tables

Scenario: Using tables
	Given I have the following persons
		| Name		| Style		| Birth date | Cred |
		| Marcus    | Cool		| 1972-10-09 | 50   |
		| Anders    | Butch		| 1977-01-01 | 500  |
		| Jocke     | Soft		| 1974-04-04 | 1000 |
	When I search for Jocke
	Then the following person should be returned
		| Name		| Style		| Birth date | Cred |
		| Jocke     | Soft		| 1974-04-04 | 1000 |

Scenario: Using tables with SpecFlow Assist
	Given I have the following persons using assist 
		| Name		| Style		| Birth date | Cred |
		| Marcus    | Very cool	| 1972-10-09 | 50   |
		| Anders    | Butch		| 1977-01-01 | 500  |
		| Jocke     | Soft		| 1974-04-04 | 1000 |
	When I search for Jocke
	Then the following person should be returned using assist
		| Name		| Style		| Birth date | Cred |
		| Jocke     | Soft		| 1974-04-04 | 1000 |

Scenario: Creating a entity from field value
	When I fill out the form like this
		| Field		 | Value		|
		| Name		 | Marcus		|
		| Style      | very cool	|
		| Birth date | 1972-10-09	|
		| Cred		 | 100			|
	Then the following person should be returned using assist
		| Name		| Style		| Birth date | Cred |
		| Marcus    | Very cool	| 1972-10-09 | 1000 |

Scenario: Example of a wide table
	Given this wide table
		| Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred | Name   | Style     | Birth date | Cred |
		| Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 | Marcus | Very cool | 10/9/1972  | 1000 |