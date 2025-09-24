using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace StudyProject.Logging
{
    public static class SerilogLoggingExtensions
    {
        public static void AddSerilogLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();
                loggingBuilder.AddSerilog();
            });
        }
    }
}
