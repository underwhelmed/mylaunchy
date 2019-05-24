using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using mylaunchy.services.common.Extensions;
using System;
using System.Linq;

namespace mylaunchy.services.launchpad
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Starting Launchpad Microservice");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {                
                logger.Error(ex, "Stopped Launchpad Microservice because of exception");                
            }
            finally
            {                
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            builder.ConfigureMicroserviceConfiguration(); 
            return builder.UseStartup<Startup>()
                .Build();
        }
    }
}