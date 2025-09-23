using Microsoft.Extensions.Options;

namespace StudyProject.Configuration
{
    public class ConfigManager
    {
        private readonly IOptions<Config> config;

        public ConfigManager(IOptions<Config> config)
        {
            this.config = config;
        }

        public Config GetConfig()
        {
            return this.config.Value;
        }

    }
}
