using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudyProject.Configuration;
using StudyProject.EFCore;
using StudyProject.Logging;

var services = new ServiceCollection();
services.AddConfiguration();
services.AddSerilogLogging();
services.AddDbContext<ApplicationDbContext>();

var serviceProvider = services.BuildServiceProvider();

var configManager = serviceProvider.GetRequiredService<ConfigManager>();
var config = configManager?.GetConfig();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogDebug("config: {@config}", config);

var ctx = serviceProvider.GetRequiredService<ApplicationDbContext>();
ctx.Persons.ToList().ForEach(person => logger.LogInformation("person {@person}", person));
