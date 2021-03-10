using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.System
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}