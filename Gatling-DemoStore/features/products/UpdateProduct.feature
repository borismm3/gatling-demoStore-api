Feature: Update a product

As a user
I want to update a product
So that I can modify its details

@PutProduct @ProductFeature
Scenario: Update a product
    Given I have the username "admin" and password "admin" to update
    And I have the product ID "20"
    And I have the following updated product details:
        | Key          | Value                |
        | name         | "Mojix updated"      |
        | description  | "Mojix updated"      |
        | image        | "mojix-updated.jpg"  |
        | price        | "29.99"              |
        | categoryId   | 7                    |
    When I send a PUT request to the API endpoint "http://demostore.gatling.io/api" to update the product
    Then the response status code for updating the product should be 200
    And the response body should contain the updated product details
