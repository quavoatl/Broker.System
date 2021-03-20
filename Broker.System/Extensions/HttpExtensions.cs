using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Broker.System.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null) return string.Empty;

            return httpContext.User.Claims
                .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
        }
    }
}