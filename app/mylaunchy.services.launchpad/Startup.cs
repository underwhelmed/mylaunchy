using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mylaunchy.core.Repository.Launchpad;
using mylaunchy.core.Services.Launchpad;
using mylaunchy.repository.spacexapi;
using mylaunchy.repository.spacexapi.Deserializers;
using mylaunchy.repository.spacexapi.Factories;
using mylaunchy.services.common.Extensions;
using System;

namespace mylaunchy.services.launchpad
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IJsonResponseDeserializer, JsonResponseDeserializer>();
            services.AddTransient<IRestClientFactory, RestClientFactory>();
            services.AddTransient<IRestRequestFactory, RestRequestFactory>();
            services.AddTransient<ILaunchpadRetrievalServices, LaunchpadRetrievalServices>();
            services.AddTransient<ILaunchpadRepository, LaunchpadRepository>();

            services.RegisterSharedServices(Configuration, "v1", GetType().Namespace);
        }
       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            app.UseSharedMicroserviceConfiguration(env, "v1", GetType().Namespace);            
        }
    }
}
