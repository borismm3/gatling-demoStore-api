Feature: List all products

As a user
I want to get a list of all products
So that I can view the available products

@GetProducts @ProductFeature
Scenario: Retrieve all products
    Given the API endpoint "http://demostore.gatling.io/api/product"
    When I send a GET request to the API endpoint
    Then the response status code for all products should be 200
    And the response body should contain a list of products
