using Broker.System.Data;
using Broker.System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.System.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BrokerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConn")));
            services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<BrokerDbContext>();

            services.AddScoped<ILimitService, LimitService>();
        }
    }
}