using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudyProject.Configuration;
using StudyProject.EFCore;
using StudyProject.Logging;
using System.Data;

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

var querable = ctx.Persons.FromSqlInterpolated($"select * from T_Persons where Age <= 30");
foreach (var item in querable)
{
    logger.LogInformation("person : {@person}", item);
}

using (var conn = ctx.Database.GetDbConnection())
{
    await conn.OpenAsync();
    var cmd = conn.CreateCommand();
    cmd.CommandText = "select Id, Name, Age, Gender from T_Persons where Age > 30";
    using (var reader = await cmd.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int age = reader.GetInt32(2);
            string gender = reader.GetString(3);

            logger.LogInformation("person : {@person}", new {
                id, name, age, gender
            });
        }
    }
}

