using Api.Contracts.HealthCheck;
using API.Data;
using API.Extensions;
using API.Installer;
using API.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Linq;

namespace API
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
            //Clean Up Di container Using Reflection
            InstallerExtension.InstallServicesInAssemby(services, Configuration);
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDbHeathCheck();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI(option => option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description));

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
