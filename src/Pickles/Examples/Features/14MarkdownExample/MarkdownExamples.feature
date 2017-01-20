Feature: Sample Markdown Feature
	\# Header 1

	\#\# Header 2

	\#\#\# Header 3

	\#\#\#\# Header 4

	\#\#\#\#\# Header 5

	\#\#\#\#\#\# Header 6

	This is a *significant* word

	1. Ordered #1
	2. Ordered #2
	3. Ordered #3

	- Unordered #1
	- Unordered #2
	- Unordered #3

	Horizontal Rule:
	- - -

	Table example:

	| First Header  | Second Header |
	| ------------- | ------------- |
	| Content Cell  | Content Cell  |
	| Content Cell  | Content Cell  |

	- - -

	Including a picture: ![](./image.png)

    Background:
	This is the *coolest* background

Given I have initialized the Sum-variable to 0
When I add 1 to the Sum-variable

    Scenario: Sample Markdown Scenario Example

	This is **important** text

	Code Block:

	```
	var x = 2;
	```

	Apple
	:   Pomaceous fruit of plants of the genus Malus in
	    the family Rosaceae.
	:   An American computer company.

	Orange
	:   The fruit of an evergreen tree of the genus Citrus.

        Given this
        Then that

    @AddingATag
    Scenario Outline: Sample Markdown Scenario Outline Example

	This is [an example link to pickles](https://github.com/picklesdoc/pickles/wiki "Pickles") inline link.

	[This link to pickles](https://github.com/picklesdoc/pickles/wiki) has no title attribute.

        Given this: <test>
        Then that: <test2>

        Examples:

	This __message__ is important too and is for an *Example* table.

            | test  | test2  |
            | value | value2 |
