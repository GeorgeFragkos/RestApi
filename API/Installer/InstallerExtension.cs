using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace API.Installer
{
    public static class InstallerExtension
    {
        public static void InstallServicesInAssemby(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.
                Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(x => x.InstallServices(configuration, services));
        }
    }
}
