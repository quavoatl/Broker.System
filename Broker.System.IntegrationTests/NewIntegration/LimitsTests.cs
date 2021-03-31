using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using FluentAssertions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Broker.System.IntegrationTests.NewIntegration
{
    public class LimitsTests : IClassFixture<TestApplicationFactory<Startup>>
    {
        private readonly TestApplicationFactory<Startup> _factory;

        public LimitsTests(TestApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private async Task<string> GetJwtFromLoginComponentAsync()
        {
            var authServerClient = _factory.CreateClient();

            var tokenResponseMessage = await authServerClient.PostAsJsonAsync("api/v1/tokenRequest", new PasswordGrantTokenRequest()
            {
                ClientId = "broker_limits_rest_client_tests",
                Secret = "secret",
                Email = "brokeras@gmail.com",
                Password = "Password1234!"
            });
            var token = await tokenResponseMessage.Content.ReadAsStringAsync();

            var deserializedJObj = (JObject) JsonConvert.DeserializeObject(token);
            var tokenVal = (JValue) deserializedJObj["tokenValue"];
            return tokenVal.Value<string>();
        }

        [Fact]
        public async Task GetAll_GivenInMemoryDatabase_ShouldReturn0Limits()
        {
            // Arrange
            var client = _factory.CreateClient();
            var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5005/");
            var accessToken = await GetJwtFromLoginComponentAsync();
            client.SetBearerToken(accessToken);
            
            // Act
            var response = await client.GetAsync(ApiRoutes.Limit.GetAll);
            var responseContent = response.Content.ReadAsStringAsync();
            
            // Assert  
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}