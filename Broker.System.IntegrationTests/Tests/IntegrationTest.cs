using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Data;
using Broker.System.IntegrationTests.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using static Microsoft.Extensions.DependencyInjection.ServiceLifetime;

namespace Broker.System.IntegrationTests.Tests
{
    public class IntegrationTest 
    {
        protected readonly HttpClient TestClient;

         public IntegrationTest()
         {
             var app = new WebApplicationFactory<Startup>()
                 .WithWebHostBuilder(builder =>
                 {
                     builder.ConfigureServices(services =>
                     {
                         // Remove database registrations
                         CleanupDatabaseRegistrations<BrokerDbContext>(services);
        
                         // Create a new service provider.
                         var serviceProvider = new ServiceCollection()
                             .AddEntityFrameworkInMemoryDatabase()
                             .BuildServiceProvider();
        
                         // Add a database context (AppDbContext) using an in-memory database for testing.
                         services.AddDbContext<BrokerDbContext>(options =>
                         {
                             options.UseInMemoryDatabase("IntegrationTests");
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
        
             TestClient = app.CreateClient();
         }
        
         protected async Task AuthenticateAsync()
         {
             TestClient.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Bearer", await GetJwtFromLoginComponentAsync());
         }
        
         private async Task<string> GetJwtFromLoginComponentAsync()
         {
             HttpResponseMessage res;
        
             using (var client = new HttpClient())
             {
                 var response = await client.PostAsJsonAsync(ApiRoutes.LoginComponentApi.Login, new
                 {
                     username = "integrationTestBroker@integration.com",
                     password = "Test1234!"
                 });
                 res = response;
             }
        
             var registrationResponse = await res.Content.ReadAsStringAsync();
        
             var deserializedJObj = (JObject) JsonConvert.DeserializeObject(registrationResponse);
             var token = (JValue) deserializedJObj["token"];
        
             return token.Value<string>();
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