using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Broker.System.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null) return string.Empty;

            return httpContext.User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
        }
    }
}