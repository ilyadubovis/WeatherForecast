namespace WeatherForecast.Model
{
    public class WeatherZoneInfo
    {
        public string Id { get; set; } = String.Empty;
        public string? Name { get; set; }
        public string? State { get; set; }

        public override bool Equals(object? obj) => 
            Id.Equals((obj as WeatherZoneInfo)?.Id, StringComparison.InvariantCultureIgnoreCase);
        public override int GetHashCode() => 
            Id.ToUpper().GetHashCode();
    }
}
