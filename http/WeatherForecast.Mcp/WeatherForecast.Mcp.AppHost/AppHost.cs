var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.WeatherForecast_Mcp_ApiService>("apiservice");

var mcp = builder.AddProject<Projects.WeatherForecast_Mcp_Server>("weatherforecast-mcp-server")
    .WithReference(apiService).WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");

builder.AddMcpInspector("mcp-inspector")
    .WithMcpServer(mcp)
    .WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");

builder.Build().Run();
