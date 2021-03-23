using System;
using System.Net.Http;
using System.Threading.Tasks;
using Broker.System.Controllers.V1.Responses;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Broker.System.Controllers
{
    public class TokenController : Controller
    {
        [HttpGet("api/v1/tokenDoc")]
        public async Task<IActionResult> TokenDoc()
        {
            string token = string.Empty;
            using (var client = new HttpClient())
            {
                var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5005/");
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = discoveryDoc.TokenEndpoint,
                    ClientId = "broker_limits_rest_client_tests",
                    
                    ClientSecret = "secret",
                    UserName = "user100@example.com",
                    Password = "Password1234!"
                });

                token = tokenResponse.AccessToken;
            }
          
            return Ok(new TokenResponseObj() {TokenValue = token});
        }
    }
}