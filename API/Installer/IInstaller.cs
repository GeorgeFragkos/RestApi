using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace API.Installer
{
    public interface IInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services);
    }
}
