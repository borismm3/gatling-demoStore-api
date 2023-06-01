Feature: Create a product

As a user
I want to create a new product
So that it can be added to the product catalog

@PostProduct @ProductFeature
Scenario: Create a new product with authentication
    Given I have the username "admin" and password "admin".
    And the API endpoint "http://demostore.gatling.io/api" to create a new product.
    And the following product data:.
        | Key         | Value            |
        | name        | "Mojix glass"    |
        | description | "Mojix Glasses"  |
        | image       | "mojix-glasses.jpg" |
        | price       | "39.99"          |
        | categoryId  | 7                |
    When I send a POST request to the API endpoint with the product data.
    Then the response status code to create a new product should be 200
    And the response body should contain the created product details.
