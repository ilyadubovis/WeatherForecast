using WeatherForecast.Model;

namespace WeatherForecast.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<ForecastItem>> GetForecast(string zoneId);
        Task<WeatherZoneInfo?> GetZoneInfo(string zoneId);
    }
}