namespace MomAndBaby
{
    public class WeatherForecast
    {
        /// <summary>
        /// Note: This class is used to generate the weather forecast data.
        /// </summary>
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
