namespace WeatherForecastAPI.Services
{
    public class WeatherService
    {
        private const string BaseUrl = "https://api.tomorrow.io/v4/weather/forecast";
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly string? _apiKey;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            _apiKey = _configuration.GetSection("WeatherApi:ApiKey").Value;
        }

        public async Task<(string? Data, string Error)> GetWeatherAsync(string location) =>
            await TryFetchWeatherData(BuildWeatherUrl(location));

        private string BuildWeatherUrl(string location) =>
            $"{BaseUrl}?location={location}&units=imperial&timesteps=1d&apikey={_apiKey}";

        private async Task<(string? Data, string Error)> TryFetchWeatherData(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? (content, null)
                    : (null, content);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
