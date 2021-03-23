using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Responses;
using Broker.System.IntegrationTests.Configuration;
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
            var token = (JArray) deserializedJObj["messages"];
        
            return token.Values<string>().ToList()[0];
        }
        
        [Fact]
        public async Task GetAll()
        {
            // Arrange
            var client = _factory.CreateClient();

            var tokenResponseMessage = await client.GetAsync("api/v1/tokenDoc");
            var token = await tokenResponseMessage.Content.ReadAsStringAsync();
            
            var deserializedJObj = (JObject) JsonConvert.DeserializeObject(token);
            var tokenVal = (JValue) deserializedJObj["tokenValue"];
           
            
            //var loginResponse = await GetJwtFromLoginComponentAsync();
            
            
            var discoveryDoc = await client.GetAsync("https://localhost:5005/.well-known/openid-configuration");
            // var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            //     new ClientCredentialsTokenRequest()
            //     {
            //         Address = discoveryDoc.TokenEndpoint,
            //         
            //         ClientId = "broker_limits_rest_client",
            //         ClientSecret = "secretsecretsecret",
            //         
            //         Scope = "ApiOne",
            //     });
            
            //retrieve secret data
            client.SetBearerToken(tokenVal.Value<string>());
            // Act
            
            
            var response = await client.GetAsync(ApiRoutes.Limit.GetAll);
            var responseContent = response.Content.ReadAsStringAsync();
            // Assert  

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}