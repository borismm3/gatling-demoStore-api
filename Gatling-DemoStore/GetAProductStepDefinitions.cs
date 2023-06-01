using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace Gatling_DemoStore
{
    [Binding]
    public class GetAProductStepDefinitions
    {
        RestClient client = new RestClient("http://demostore.gatling.io/api");
        RestRequest request = new RestRequest("/product/{postid}", Method.Get);
        RestResponse response;

        [Given(@"I have a valid product ID")]
        public void GivenIHaveAValidProductID()
        {
            request.AddUrlSegment("postid", 20);
        }

        [When(@"I send a GET request to the ""([^""]*)"" endpoint")]
        public void WhenISendAGETRequestToTheEndpoint(string p0)
        {
            response = client.Execute(request);
        }

        [Then(@"the response status code for get a product should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int p0)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"the response body should contain the product details")]
        public void ThenTheResponseBodyShouldContainTheProductDetails()
        {
            var content = response.Content;
            var jsonObject = JObject.Parse(content);
            string result = jsonObject.SelectToken("categoryId").ToString();
            Console.WriteLine("content = " + content);
            Assert.That(result, Is.EqualTo("5"), "Error! The category id is not correct");
        }
    }
}
