namespace Mcp.WeatherForecast.Model
{
    /// <summary>
    /// Represents a weather forecast for a specific date, including temperature and summary information.
    /// </summary>
    /// <remarks>This class provides properties for both Celsius and Fahrenheit temperatures, as well as an
    /// optional textual summary. It is commonly used to model weather data in applications such as weather services or
    /// dashboards.</remarks>
    public class WeatherForecast
    {
        /// <summary>
        /// Gets or sets the date associated with this instance.
        /// </summary>
        public DateTime Date { get; set; }

       
        /// <summary>
        /// Gets or sets the temperature in degrees Celsius.
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Gets the temperature in degrees Fahrenheit, calculated from the Celsius value.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Gets or sets the summary text associated with the object.
        /// </summary>
        public string? Summary { get; set; }
    }
}
