using API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Installer
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHealthChecks().AddDbContextCheck<DataContext>();
        }
    }
}
