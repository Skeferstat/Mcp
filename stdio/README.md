

1. Build and start Api   

2.  Build server   

    ```bash  
    Mcp.WeatherForecast.Server> dotnet build
    ```
	
3. Mcp-Inspector

   ```bash 
   npx @modelcontextprotocol/inspector dotnet run
   ```

3. Postman

    New Request &rarr; MCP &rarr; Path to the mcp-ser-ver.exe   
    Then test endpoints.   

4. VS Code

    **Strg + Shift +P** &rarr; Add MCP Server or

    add mcp.json file to .vscode folder

    ```json  
    {
        "servers": {
            "FF-Mcp-WeatherForecast": {
                "type": "stdio",
                "command": "dotnet",
                "args": [
                    "run",
                    "--project",
                    "./Mcp.WeatherForecast.Server/Mcp.WeatherForecast.Server.csproj"
                ]
            }
        },
        "inputs": []
    }
    ```


4. Copilot

    Give me the weather forecast
    Add a forecast for 01.06.2026. Set temparature to -99 Celcius and the summary to cold. 
    Update forecast with index 4.  Set temparature to -11 Celcius and the summary to cold. 