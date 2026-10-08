@api
Feature: Find pets by status

Scenario Outline: Find pets by status
	Given I am authenticated and authorised
	When I GET a list of '<status>' pets
	Then the response code should be 200
	And the status of each pet returned should be <status>

Examples:
	| status    |
	| available |
	| sold      |
	| pending   |

