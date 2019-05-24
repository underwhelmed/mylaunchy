using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace mylaunchy.services.common.Extensions
{
    public static class WebHostingExtensions
    {
        public static IWebHostBuilder ConfigureMicroserviceConfiguration(this IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            });
            builder.UseNLog();
            return builder;
        }
    }
}
