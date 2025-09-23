using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StudyProject.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void AddConfiguration(
            this IServiceCollection services, string configPath = "config.json")
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, configPath));

            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configurationRoot);
            services.AddScoped<ConfigManager>();
            services.AddOptions().Configure<Config>(config => configurationRoot.Bind(config));
        }
    }
}
