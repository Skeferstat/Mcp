using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace Mcp.WeatherForecast.Server
{
    /// <summary>
    /// Provides static methods for retrieving, creating, and updating weather forecast data from the server.
    /// </summary>
    /// <remarks>This class is intended for use with server-side weather forecast operations and exposes
    /// asynchronous methods for interacting with weather forecast resources. All methods perform HTTP requests to the
    /// configured server endpoint and ensure successful responses before processing data. The class cannot be
    /// instantiated.</remarks>
    [McpServerToolType]
    public static class WeatherForecastTools
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44394/")
        };

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Asynchronously retrieves the current weather forecast data from the server.
        /// </summary>
        /// <remarks>The method performs an HTTP GET request to obtain weather forecast information. The
        /// returned list will be empty if the server response contains no forecast data. This method does not throw an
        /// exception for unsuccessful HTTP responses; instead, it ensures a successful status code before processing
        /// the response.</remarks>
        /// <returns>A list of <see cref="Mcp.WeatherForecast.Model.WeatherForecast"/> objects representing the current weather
        /// forecast. Returns an empty list if no forecast data is available.</returns>
        [McpServerTool, Description("Retrieves the current weather forecast.")]
        public static async Task<List<Mcp.WeatherForecast.Model.WeatherForecast>> GetWeatherForecastAsync()
        {
            var response = await _httpClient.GetAsync("WeatherForecast");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Fix CS8603 by using null-coalescing operator to return an empty list if deserialization returns null
            return JsonSerializer.Deserialize<List<Model.WeatherForecast>>(json, JsonOptions) ?? [];
        }


        /// <summary>
        /// Creates a new weather forecast entry asynchronously.
        /// </summary>
        /// <param name="forecast">The weather forecast data to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a message indicating that the
        /// weather forecast was created successfully.</returns>
        [McpServerTool, Description("Creates a new weather forecast entry.")]
        public static async Task<string> CreateWeatherForecastAsync(Mcp.WeatherForecast.Model.WeatherForecast forecast)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(forecast), Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
            var response = await _httpClient.PostAsync("WeatherForecast", jsonContent);
            response.EnsureSuccessStatusCode();

            return "Weather forecast created successfully.";
        }


        /// <summary>
        /// Asynchronously updates an existing weather forecast entry with the specified data.
        /// </summary>
        /// <param name="index">The index of the forecast to update.</param>
        /// <param name="forecast">The weather forecast model containing the updated information to be applied. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a message indicating that the
        /// weather forecast was updated successfully.</returns>
        [McpServerTool, Description("Updates an existing weather forecast entry.")]
        public static async Task<string> UpdateWeatherForecastAsync(int index, Mcp.WeatherForecast.Model.WeatherForecast forecast)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(forecast), Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
            var response = await _httpClient.PutAsync($"WeatherForecast/{index}", jsonContent);
            response.EnsureSuccessStatusCode();

            return "Weather forecast updated successfully.";
        }
    }
}
