using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace Gatling_DemoStore
{
    [Binding]
    public class UpdateAProductStepDefinitions
    {
        private RestClient client;
        private RestRequest authenticationRequest;
        private RestResponse authenticationResponse;
        private RestRequest productRequest;
        private RestResponse productResponse;
        private JObject productData;
        private string bearerToken;
        private string productID;

        [Given(@"I have the username ""([^""]*)"" and password ""([^""]*)""\ to update")]
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

        [Given(@"I have the product ID ""([^""]*)""")]
        public void GivenIHaveTheProductID(string id)
        {
            this.productID = id;
        }

        [Given(@"I have the following updated product details:")]
        public void GivenIHaveTheFollowingUpdatedProductDetails(Table table)
        {
            productData = new JObject();
            foreach (var row in table.Rows)
            {
                string key = row["Key"];
                string value = row["Value"];
                productData[key] = JToken.Parse(value);
            }
        }

        [When(@"I send a PUT request to the API endpoint ""([^""]*)"" to update the product")]
        public void WhenISendAPUTRequestToTheAPIEndpointToUpdateTheProduct(string endpoint)
        {
            client = new RestClient(endpoint);
            productRequest = new RestRequest($"product/{productID}", Method.Put);
            productRequest.AddHeader("Authorization", $"Bearer {bearerToken}");
            productRequest.AddJsonBody(new
            {
                name = "Mojix updated",
                description = "Mojix updated",
                image = "mojix-updated.jpg",
                price = "29.99",
                categoryId = 7
            });
            productResponse = client.Execute(productRequest);
        }

        [Then(@"the response status code for updating the product should be (.*)")]
        public void ThenTheResponseStatusCodeForUpdatingTheProductShouldBe(int p0)
        {
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = productResponse.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode, $"Expected status code: {expectedStatusCode}, but received: {actualStatusCode}");
        }

        [Then(@"the response body should contain the updated product details")]
        public void ThenTheResponseBodyShouldContainTheUpdatedProductDetails()
        {
            var content = productResponse.Content;
            var updatedProduct = JObject.Parse(content);

            Assert.That(updatedProduct["name"].ToString(), Is.EqualTo(productData["name"].ToString()));
            Assert.That(updatedProduct["description"].ToString(), Is.EqualTo(productData["description"].ToString()));
            Assert.That(updatedProduct["image"].ToString(), Is.EqualTo(productData["image"].ToString()));
            Assert.That(updatedProduct["price"].ToString(), Is.EqualTo(productData["price"].ToString()));
            Assert.That(updatedProduct["categoryId"].ToString(), Is.EqualTo(productData["categoryId"].ToString()));
        }
    }
}