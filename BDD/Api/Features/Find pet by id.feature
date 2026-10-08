@api
Feature: Find pet by Id

Background:
    Given a valid pet Id
    And I am authenticated and authorised

Scenario: Find pet by Id	
    When I GET a pet by id
    Then the response code should be 200
    And the pet with the matching id should be returned	
