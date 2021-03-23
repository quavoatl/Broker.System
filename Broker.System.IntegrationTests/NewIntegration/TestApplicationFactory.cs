using System.IO;
using Broker.System.Filters;
using Broker.System.IntegrationTests.Configuration;
using Broker.System.IntegrationTests.Tests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Broker.System.IntegrationTests.NewIntegration
{
    public class TestApplicationFactory<TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHost(builder =>
                {
                    builder.UseStartup<TTestStartup>();
                    builder.ConfigureTestServices(services =>
                    {
                        // services.AddAuthentication("Test")
                        //     .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", opt => { });
                        //
                    });
                });

            return host;
        }
    }
}