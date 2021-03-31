using System;
using System.Net.Http;
using System.Threading.Tasks;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Broker.System.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost("api/v1/tokenRequest")]
        public async Task<IActionResult> TokenDoc([FromBody] PasswordGrantTokenRequest tokenRequest)
        {
            string token = string.Empty;
            using (var client = new HttpClient())
            {
                var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5005/");
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = discoveryDoc.TokenEndpoint,

                    ClientId = tokenRequest.ClientId,
                    ClientSecret = tokenRequest.Secret,
                    UserName = tokenRequest.Email,
                    Password = tokenRequest.Password
                });

                token = tokenResponse.AccessToken;
            }

            return Ok(new TokenResponseObj() {TokenValue = token});
        }
    }
}