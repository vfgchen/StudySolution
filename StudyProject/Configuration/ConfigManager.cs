using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;

namespace StudyProject.Configuration
{
    public class ConfigManager
    {
        public IOptions<Config> Config { get; }

        public ConfigManager(IOptions<Config> config)
        {
            this.Config = config;
        }

    }
}
