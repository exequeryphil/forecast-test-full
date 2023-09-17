using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using WeatherForecastAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using RichardSzalay.MockHttp;

namespace WeatherForecastAPI.Tests
{
    [TestClass]
    public class WeatherControllerTests
    {

        [TestMethod]
        public async Task Get_ReturnsExpectedErrorResponse_WhenApiReturnsBadRequest()
        {
            const string location = "N0wh3r3";
            const string apiMatch = "https://api.tomorrow.io/*";

            var mockHttp = new MockHttpMessageHandler();

            // We'll mock a request that sends a bad location and will expect a 400 bad request error back
            mockHttp.When(apiMatch)
                .Respond(HttpStatusCode.BadRequest, "application/json", "{\"code\": 400001,\"type\": \"Invalid Query Parameters\",\"message\": \"The entries provided as query parameters were not valid for the request. Fix parameters and try again: 'location' - failed to query by the term 'N0wh3r3', try a different term\"}");
            var client = new HttpClient(mockHttp);
            var weatherController = new WeatherController(client);
            var actionResult = await weatherController.Get(location);

            var objectResult = actionResult as ObjectResult;
            var jsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(objectResult?.Value);

            // Expecting the controller to return 500 with the external API error data inside an "apiError" property
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.StatusCode, 500);
            Assert.IsTrue(jsonValue.Contains("apiError"));

        }
    }
}
