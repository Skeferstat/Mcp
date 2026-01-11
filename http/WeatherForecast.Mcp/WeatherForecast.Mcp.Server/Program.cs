using System.Net.Http.Headers;
using WeatherForecast.Mcp.Server.Tools;

namespace WeatherForecast.Mcp.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddCors(options => // cors is required for mcp inspector with oauth
        {
            options.AddPolicy("DevAll", policy => policy
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
        });


        builder.Services.AddMcpServer()
               .WithHttpTransport()
               .WithTools<WeatherTools>();

        // Configure HttpClientFactory for weather.gov API
        builder.Services.AddHttpClient("WeatherApi", client =>
        {
            client.BaseAddress = new Uri("https://api.weather.gov");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weather-tool", "1.0"));
        });

        // Use service discovery for the ApiService
        builder.Services.AddHttpClient("RandomWeatherForecastApi", client =>
        {
            client.BaseAddress = new Uri("http://apiservice");
        });

        var app = builder.Build();

        app.UseCors("DevAll");
        
        app.MapDefaultEndpoints();
        app.MapMcp();
       
        app.Run();
    }
}
