Feature: Interactive DHTML View
	In order to increase stakeholder engagement with pickled specs  
	As a SpecFlow evangelist  
	I want to adjust the level of detail in the DHTML view to suite my audience  
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
