using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using TechTalk.SpecFlow;
using static System.Net.Mime.MediaTypeNames;

namespace Gatling_DemoStore
{
    [Binding]
    public class CreateAProductStepDefinitions
    {
        private RestClient client;
        private RestRequest authenticationRequest;
        private RestResponse authenticationResponse;
        private RestRequest productRequest;
        private RestResponse productResponse;
        private JObject productData;
        private string bearerToken;

        [Given(@"I have the username ""([^""]*)"" and password ""([^""]*)""\.")]
        public void GivenIHaveTheUsernameAndPassword(string username, string password)
        {
            client = new RestClient("http://demostore.gatling.io/api");
            authenticationRequest = new RestRequest("authenticate", Method.Post);
            authenticationRequest.AddJsonBody(new
            {
                username = username,
                password = password
            });

            authenticationResponse = client.Execute(authenticationRequest);

            var jsonResponse = JObject.Parse(authenticationResponse.Content);
            bearerToken = jsonResponse["token"].ToString();
        }

        [Given(@"the API endpoint ""([^""]*)"" to create a new product\.")]
        public void GivenTheAPIEndpointToCreateANewProduct_(string endpoint)
        {
            client = new RestClient(endpoint);
            productRequest = new RestRequest("product", Method.Post);
            productRequest.AddHeader("accept", "*/*");
            productRequest.AddHeader("Authorization", $"Bearer {bearerToken}");
            productRequest.AddHeader("Content-Type", "application/json");
        }

        [Given(@"the following product data:\.")]
        public void GivenTheFollowingProductData_(Table table)
        {
            productData = new JObject();
            foreach (var row in table.Rows)
            {
                string key = row["Key"];
                string value = row["Value"];

                productData[key] = JToken.Parse(value);
            }
        }

        [When(@"I send a POST request to the API endpoint with the product data\.")]
        public void WhenISendAPOSTRequestToTheAPIEndpointWithTheProductData_()
        {
            productRequest.AddJsonBody(new
            {
                name = "Mojix glass",
                description = "Mojix Glasses",
                image = "mojix-glasses.jpg",
                price = "39.99",
                categoryId = 7
            });
            productResponse = client.Execute(productRequest);
        }

        [Then(@"the response status code to create a new product should be (.*)")]
        public void ThenTheResponseStatusCodeToCreateANewProductShouldBe(int p0)
        {
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = productResponse.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode, $"Expected status code: {expectedStatusCode}, but received: {actualStatusCode}");
        }

        [Then(@"the response body should contain the created product details\.")]
        public void ThenTheResponseBodyShouldContainTheCreatedProductDetails_()
        {
            var content = productResponse.Content;
            var createdProduct = JObject.Parse(content);

            Assert.That(createdProduct["name"].ToString(), Is.EqualTo(productData["name"].ToString()));
            Assert.That(createdProduct["description"].ToString(), Is.EqualTo(productData["description"].ToString()));
        }
    }
}
