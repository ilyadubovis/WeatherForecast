using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;
using WeatherForecast.Services;
using File = System.IO.File;

namespace WeatherForecast.Test
{
    [TestClass]
    public class WeatherForecastTest
    {
        private WeatherForecastService _weatherForecastService = default!;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _weatherForecastService = new(new HttpClient(), new LoggerFactory());
        }

        [TestMethod]
        public void TestValidForecast()
        {
            var jsonNode = JsonNode.Parse(File.ReadAllText(@"test-data/forecast-valid.json"));
            var forecasts = _weatherForecastService.GetForecast(jsonNode);

            Assert.IsTrue(forecasts.Any());
        }

        [TestMethod]
        public void TestInvalidForecast()
        {
            var jsonNode = JsonNode.Parse(File.ReadAllText(@"test-data/forecast-invalid.json"));
            var forecasts = _weatherForecastService.GetForecast(jsonNode);

            Assert.IsTrue(!forecasts.Any());
        }
    }
}