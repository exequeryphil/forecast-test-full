using Microsoft.AspNetCore.Mvc;
using WeatherForecastAPI.Services;

namespace WeatherForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(HttpClient httpClient)
        {
            _weatherService = new WeatherService(httpClient);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string location)
        {
            var (jsonResponse, apiError) = await _weatherService.GetWeatherAsync(location);
            return DetermineResponse(jsonResponse, apiError);
        }

        private IActionResult DetermineResponse(string jsonResponse, string apiError)
        {
            if (!string.IsNullOrEmpty(apiError))
                return StatusCode(500, new { apiError });

            if (string.IsNullOrEmpty(jsonResponse))
                return NotFound(new { apiError = "No data available or not found" });

            return Content(jsonResponse, "application/json");
        }


    }
}