using Broker.System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbContext = Broker.System.Data.DbContext;

namespace Broker.System.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext>(options =>
                SqlServerDbContextOptionsExtensions.UseSqlServer(options));
            services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<DbContext>();

            services.AddSingleton<ILimitService, LimitService>();
        }
    }
}