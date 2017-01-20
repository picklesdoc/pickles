Feature: Show the compare to feature
	In order to show the compare to features of SpecFlow Assist  
	As a SpecFlow evanglist  
	I want to show how the different versions of compareTo works


#Yes - this is a bug with the date format...
Scenario: CompareToInstance
	Given I have the following person
		| Field		 | Value		|
		| Name		 | Marcus		|
		| Style      | Butch		|
		| Birth date | 1972-10-09	|
	Then CompareToInstance should match this guy
		| Field		 | Value					|
		| Name		 | Marcus					|
		| Style      | Butch					|
		| BirthDate  | 10/9/1972 12:00:00 AM	| 
	And CompareToInstance should match this guy
		| Field		 | Value					|
		| Name		 | Marcus					|
		| BirthDate	 | 10/9/1972 12:00:00 AM	|
	But CompareToInstance should not match this guy
		| Field		 | Value					|
		| Name		 | Anders					|
		| Style      | very cool				|
		| BirthDate	 | 10/9/1974 12:00:00 AM	|

# CompareToSet will test only the properties that you define in the table. 
# CompareToSet does not test the order of the objects, only that one was found that matches
Scenario: CompareToSet 
	Given I have the following persons using assist 
		| Name		| Style		| Birth date |
		| Marcus    | Cool		| 1972-10-09 | 
		| Anders    | Butch		| 1977-01-01	 | 
		| Jocke     | Soft		| 1974-04-04 | 
	Then CompareToSet should match this
		| Name		| Style		| BirthDate				| 
		| Marcus    | Cool		| 10/9/1972 12:00:00 AM | 
		| Anders    | Butch		| 1/1/1977 12:00:00 AM	| 
		| Jocke     | Soft		| 4/4/1974 12:00:00 AM	| 
	But CompareToSet should not match this
		| Name		| Style		| BirthDate				|  
		| Marcus    | Cool		| 10/9/1972 12:00:00 AM |		
		| Anders    | Butch		| 1/1/1977 12:00:00 AM	|