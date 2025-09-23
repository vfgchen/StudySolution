using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyProject.Configuration;

var services = new ServiceCollection();
services.AddConfiguration();
var serviceProvider = services.BuildServiceProvider();

var configManager = serviceProvider.GetService<ConfigManager>();
var config = configManager?.Config.Value;
Console.WriteLine(config?.Contact?.Email);
