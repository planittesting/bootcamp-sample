@web
Feature: Subscribing to AEMO updates

  Scenario: Navigate to the subscription page via Subscribe link
    Given a visitor is on the AEMO home page
    When they choose to subscribe
    Then the subscription page is displayed
