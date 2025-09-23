using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using StudyProject.Configuration;
using System.Text.Json;

var services = new ServiceCollection();
services.AddConfiguration();
services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();
    loggingBuilder.AddSerilog();
});
var serviceProvider = services.BuildServiceProvider();

var configManager = serviceProvider.GetRequiredService<ConfigManager>();
var config = configManager?.GetConfig();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogDebug($"config: {JsonSerializer.Serialize(config)}");