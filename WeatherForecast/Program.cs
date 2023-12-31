using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherForecast;
using WeatherForecast.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

await builder.Build().RunAsync();
