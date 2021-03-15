using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;
using Broker.System.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Broker.System.Controllers.V1
{
    [EnableCors(policyName: "mata")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest registrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            HttpResponseMessage res;

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(ApiRoutes.LoginComponentApi.Register, new
                {
                    email = registrationRequest.Email,
                    username = registrationRequest.Email,
                    password = registrationRequest.Password,
                    isBroker = true
                });
                res = response;
            }

            var registrationResponse = await res.Content.ReadAsStringAsync();

            var deserializedJObj = (JObject) JsonConvert.DeserializeObject(registrationResponse);
            var errors = deserializedJObj["errors"].Values<string>();
            var status = deserializedJObj["status"].Value<string>();

            AuthenticationResult authResponse = new AuthenticationResult()
            {
                Errors = errors
            };

            if (status.Equals("Created"))
            {
                return Ok(new RegistrationResponse()
                {
                    Status = status
                });
            }

            return BadRequest(new AuthFailedResponse()
            {
                Errors = authResponse.Errors
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            HttpResponseMessage res;
            
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(ApiRoutes.LoginComponentApi.Login, new
                {
                    username = loginRequest.Email,
                    password = loginRequest.Password
                });
                res = response;
            }
            
            var loginResponse = await res.Content.ReadAsStringAsync();
            
            var deserializedJObj = (JObject) JsonConvert.DeserializeObject(loginResponse);
            var messages = deserializedJObj["messages"].Values<string>();
            var status = deserializedJObj["success"].Value<bool>();

            LoginResponse authResponse = new LoginResponse()
            {
                Messages = messages,
                Success = status
            };

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = authResponse.Messages
                });
            }

            return Ok(new AuthSuccessResponse()
            {
                Token = authResponse.Messages.ToList()[0],
                RefreshToken = authResponse.Messages.ToList()[1]
            });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var authResponse =
                await _identityService.RefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse()
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}