@web
Feature: Subscription form validation

Rule: Required subscriber details must be provided

Scenario: Visitor submits empty subscription form
	Given a visitor is on the Subscribe page
	When they submit an empty subscription form
	Then an error is shown for each required field:
		| Field Name |
		| First Name |
		| Last Name  |
		| Email      |

Scenario Outline: Visitor submits empty subscription form - approach 2
	Given a visitor is on the Subscribe page
	When they submit an empty subscription form
	Then an error is shown for required field "<Field Name>"

Examples:
	| Field Name |
	| First Name |
	| Last Name  |
	| Email      |