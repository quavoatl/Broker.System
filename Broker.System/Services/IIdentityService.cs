using System.Threading.Tasks;
using Broker.System.Domain;

namespace Broker.System.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}