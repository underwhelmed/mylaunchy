using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace mylaunchy.services.common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSharedServices(this IServiceCollection services, IConfiguration configuration, string apiVersion, string apiName)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            
            services.AddSwaggerGen(c => {
                c.SwaggerDoc(apiVersion, new Info { Title = $"{apiName} API", Version = apiVersion});
                c.EnableAnnotations();
            });
            AddNLog(services);                    
            return services;
        }

        static void AddNLog(IServiceCollection services)
        {
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            var nLogConfig = "nlog.config";
#if DEBUG
            if (!System.IO.File.Exists(nLogConfig))
                nLogConfig = $"../../../../mylaunchy.services.common/{nLogConfig}";
#endif
            NLog.LogManager.LoadConfiguration(nLogConfig);
        }
    }
}
