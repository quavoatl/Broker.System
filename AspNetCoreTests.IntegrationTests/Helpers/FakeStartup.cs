using System;
using Broker.System.Contracts.V1;
using Broker.System.Data;
using Broker.System.Domain;
using Broker.System.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreTests.IntegrationTests
{
    public class FakeStartup //: Startup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BrokerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookies";
                    config.DefaultChallengeScheme = "oidc";
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5005";
                    options.Audience = "broker_limits_rest_client";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://localhost:5005";
                    config.ClientId = "broker_limits_rest_client";
                    config.ClientSecret = "secret";
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    config.GetClaimsFromUserInfoEndpoint = true;

                    config.Scope.Add(ClaimsHelpers.ROLES_KEY);
                    config.ClaimActions.MapUniqueJsonKey(ClaimsHelpers.ROLE, ClaimsHelpers.ROLE, ClaimsHelpers.ROLE);
                    config.TokenValidationParameters.RoleClaimType = ClaimsHelpers.ROLE;
                });

            services.AddAuthorization();
            services.AddControllersWithViews();

            services.AddScoped<ILimitService, LimitService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<BrokerDbContext>();
                if (dbContext == null)
                {
                    throw new NullReferenceException("Cannot get instance of dbContext");
                }

                if (dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("live.db"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                //dbContext.Database.EnsureDeleted();
                // dbContext.Database.EnsureCreated();
                //
                // dbContext.Limits.Add(new Limit() {BrokerId = "123", LimitId = 1, Value = 10, CoverType = "asd"});
                // dbContext.Limits.Add(new Limit {BrokerId = "321", LimitId = 2, Value = 10, CoverType = "dsa"});
                // dbContext.SaveChanges();
            }
        }
    }
}