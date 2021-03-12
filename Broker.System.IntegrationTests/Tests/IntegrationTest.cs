using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                  
                        services.RemoveAll(typeof(BrokerDbContext));
                        services.AddDbContext<BrokerDbContext>(
                            opt => opt.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()),Scoped,ServiceLifetime.Scoped);
                        
                        
                    });
                });

            TestClient = app.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest()
            {
                Email = "test3@integration.test",
                Password = "Password1234!"
            });

            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();

            return registrationResponse.Token;
        }
    }
}