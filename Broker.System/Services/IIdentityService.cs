using System.Threading.Tasks;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;
using Microsoft.AspNetCore.Http;

namespace Broker.System.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        void WriteCookie(string cookieKey, string cookieValue, HttpResponse response);
    }
}