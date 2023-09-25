using System.Text.Json.Nodes;
using System.Text.Json;
using WeatherForecast.Model;
using System.Net.Http.Json;

namespace WeatherForecast.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly HashSet<WeatherZoneInfo> _cachedWeatherZones = new();

        public WeatherForecastService(HttpClient httpClient, ILoggerFactory LoggerFactory)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.weather.gov/");
            _logger = LoggerFactory.CreateLogger<WeatherForecastService>();
        }

        public async Task<IEnumerable<ForecastItem>> GetForecast(string zoneId) =>
             GetForecast(await _httpClient.GetFromJsonAsync<JsonNode>($"/zones/public/{zoneId}/forecast"));
        
        public IEnumerable<ForecastItem> GetForecast(JsonNode? node)
        {
            List<ForecastItem> forecasts = new();
            try
            {
                var jsonForecastItems = node?["properties"]?["periods"]?.AsArray();
                if (jsonForecastItems == default)
                {
                    throw new FormatException("Invalid weather forecast data format.");
                }
                foreach (var jsonForecastItem in jsonForecastItems)
                {
                    ForecastItem? forecast = jsonForecastItem.Deserialize<ForecastItem>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (forecast != default)
                    {
                        forecasts.Add(forecast);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetForecast", node);
            }

            return forecasts;
        }

        public async Task<WeatherZoneInfo?> GetZoneInfo(string zoneId)
        {
            var zoneMetadata = _cachedWeatherZones.SingleOrDefault(z => z.Id.Equals(zoneId, StringComparison.InvariantCultureIgnoreCase));
            if (zoneMetadata == default)
            {
                zoneMetadata = GetZoneInfo(await _httpClient.GetFromJsonAsync<JsonNode>($"/zones/public?id={zoneId}"), zoneId);
            }
            return zoneMetadata;
        }

        public WeatherZoneInfo? GetZoneInfo(JsonNode? node, string zoneId)
        {
            WeatherZoneInfo zoneMetadata;
            try
            {
                var features = node?["features"]?.AsArray();
                if (features != default)
                {
                    var metadata = features.FirstOrDefault()?["properties"];
                    if (metadata != default)
                    {
                        zoneMetadata = new() { Id = zoneId, Name = metadata["name"]?.GetValue<string>(), State = metadata["state"]?.GetValue<string>() };
                        _cachedWeatherZones.Add(zoneMetadata);
                        return zoneMetadata;    
                    }
                }
                else
                {
                    throw new FormatException("Invalid weather zone info data format.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetZoneInfo", zoneId);
                return default;
            }

            return default;
        }
    }
}
