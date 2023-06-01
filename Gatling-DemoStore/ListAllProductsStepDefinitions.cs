using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace Gatling_DemoStore
{
    [Binding]
    public class ListAllProductsStepDefinitions
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        [Given(@"the API endpoint ""([^""]*)""")]
        public void GivenTheAPIEndpoint(string endpoint)
        {
            client = new RestClient(endpoint);
            request = new RestRequest(endpoint, Method.Get);
        }

        [When(@"I send a GET request to the API endpoint")]
        public void WhenISendAGETRequestToTheAPIEndpoint()
        {
            response = client.Execute(request);
        }

        [Then(@"the response status code for all products should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int p0)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"the response body should contain a list of products")]
        public void ThenTheResponseBodyShouldContainAListOfProducts()
        {
            var content = response.Content;
            JArray jsonArray = JArray.Parse(content);
            Assert.That(jsonArray.Count, Is.GreaterThan(0), "Error! There are no items");
        }
    }
}
