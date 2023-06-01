Feature: Get a product

As a user of the DemoStore API
I want to retrieve a product by its ID
So that I can view its details

@GetAProduct @ProductFeature
Scenario: Get a product by ID
	Given I have a valid product ID
	When I send a GET request to the "/api/product/{productId}" endpoint
	Then the response status code for get a product should be 200
	And the response body should contain the product details
