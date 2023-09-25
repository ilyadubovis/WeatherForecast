using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;
using WeatherForecast.Services;
using File = System.IO.File;

namespace WeatherForecast.Test
{
    [TestClass]
    public class WeatherZoneInfoTest
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
            var jsonNode = JsonNode.Parse(File.ReadAllText(@"test-data/zoneinfo-valid.json"));
            var zoneInfo = _weatherForecastService.GetZoneInfo(jsonNode, "ORZ006");

            Assert.IsNotNull(zoneInfo);
        }

        [TestMethod]
        public void TestInvalidForecast()
        {
            var jsonNode = JsonNode.Parse(File.ReadAllText(@"test-data/zoneinfo-invalid.json"));
            var zoneInfo = _weatherForecastService.GetZoneInfo(jsonNode, "ORZ999");

            Assert.IsNull(zoneInfo);
        }
    }
}