using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Broker.System.Data;
using Broker.System.Filters;
using Broker.System.IntegrationTests.Tests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Broker.System.IntegrationTests.NewIntegration
{
    public class TestApplicationFactory<TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    {
        protected readonly HttpClient TestClient;

        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHost(builder =>
                {
                    builder.UseStartup<TTestStartup>();
                    builder.ConfigureTestServices(services =>
                    {
                        CleanupDatabaseRegistrations<BrokerDbContext>(services);

                        // Create a new service provider.
                        var serviceProvider = new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider();

                        // Add a database context (AppDbContext) using an in-memory database for testing.
                        services.AddDbContext<BrokerDbContext>(options =>
                        {
                            options.UseInMemoryDatabase($"IntegrationTests{Guid.NewGuid().ToString()}");
                            options.UseInternalServiceProvider(serviceProvider);
                        });

                        // BuildDetails the service provider.
                        var sp = services.BuildServiceProvider();

                        // Create a scope to obtain a reference to the database contexts
                        using var scope = sp.CreateScope();
                        var scopedServices = scope.ServiceProvider;
                        var appDb = scopedServices.GetRequiredService<BrokerDbContext>();

                        // Ensure the database is created.
                        appDb.Database.EnsureCreated();
                    });
                });
           
            return host;
        }

        /// <summary>
        /// Cleanup EF Core service registrations
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        private void CleanupDatabaseRegistrations<TDbContext>(IServiceCollection services) where TDbContext : DbContext
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TDbContext));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }
}