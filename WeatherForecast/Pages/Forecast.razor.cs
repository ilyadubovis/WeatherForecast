using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WeatherForecast.Model;
using WeatherForecast.Services;

namespace WeatherForecast.Pages
{
    public partial class Forecast
    {
        [RegularExpression("[a-zA-Z]{2}[zZ][0-9]{3}", ErrorMessage = "Weather Zone Id must match the pattern: [State:2]Z[Number:3]")]
        public string CurrentWeatherZoneId { get; set; } = String.Empty;
     
        private List<ForecastItem> _forecasts = new List<ForecastItem>();
        private WeatherZoneInfo? _zoneInfo;

        private bool _isInvalidWeatherZone = false;
        
        [Inject]
        private IWeatherForecastService ForecastService { get; set; } = default!;

        private async Task OnVaildSubmission(EditContext editContext)
        {
            _isInvalidWeatherZone = false;
            if (!string.IsNullOrWhiteSpace(CurrentWeatherZoneId))
            {
                CurrentWeatherZoneId = CurrentWeatherZoneId.ToUpper();
                _zoneInfo = await ForecastService.GetZoneInfo(CurrentWeatherZoneId);
                _isInvalidWeatherZone = _zoneInfo == default;
                if(_isInvalidWeatherZone)
                {
                    _forecasts.Clear();
                }
                else 
                {
                    _forecasts = (await ForecastService.GetForecast(CurrentWeatherZoneId)).ToList();
                }
            }
            else
            {
                _forecasts.Clear();
            }
        }

        private void OnInvaildSubmission(EditContext editContext)
        {
            _zoneInfo = null;
            _forecasts.Clear();
        }
    }
}
