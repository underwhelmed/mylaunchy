using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;

namespace mylaunchy.services.common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSharedMicroserviceConfiguration(this IApplicationBuilder app, IHostingEnvironment env, string apiVersion, string apiName)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"{apiName} API");                
            });

            return app;
        }
    }
}
