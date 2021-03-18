using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Broker.System.Installers.CustomMiddleware
{
    public class TokenExpiredMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExpiredMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var jwtTokenValue = context.Request.Cookies[ApiRoutes.LoginComponentApi.JwtTokenCookieKey];
            var refreshTokenValue = context.Request.Cookies[ApiRoutes.LoginComponentApi.RefreshTokenCookieKey];
            var jwtReceived = context.Request.Headers.TryGetValue("Authorization", out var jwt);

            if (jwtReceived)
            {
                context.Request.Headers.Remove("Authorization");
                context.Request.Headers.Add("Authorization", $"Bearer {jwtTokenValue}");
            }
            
            HttpResponseMessage res;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(ApiRoutes.LoginComponentApi.Refresh, new
                {
                    token = jwtTokenValue,
                    refreshToken = refreshTokenValue
                });
                res = response;
            }

            if (res.StatusCode == HttpStatusCode.OK)
            {
                var refreshReponse = await res.Content.ReadAsStringAsync();
                var deserializedJObj = (JObject) JsonConvert.DeserializeObject(refreshReponse);

                deserializedJObj.TryGetValue("messages", out var authResponse);
                var array = authResponse.Values<string>().ToList();
                string token = array[0];
                string refreshToken = array[1];

                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(refreshToken))
                {
                    context.Response.Cookies.Delete(ApiRoutes.LoginComponentApi.JwtTokenCookieKey);
                    context.Response.Cookies.Delete(ApiRoutes.LoginComponentApi.RefreshTokenCookieKey);
                    
                    var identityService =
                        (IdentityService) context.RequestServices.GetService(typeof(IIdentityService));
                    identityService.WriteCookie(ApiRoutes.LoginComponentApi.JwtTokenCookieKey, token, context.Response);
                    identityService.WriteCookie(ApiRoutes.LoginComponentApi.RefreshTokenCookieKey, refreshToken, context.Response);
                }
            }

            await _next(context);
        }
    }
}
