Feature: Create a pet

@api
Scenario: Create a pet
	Given I am authenticated and authorised
    When I POST a new pet    
    Then the pet should be created
