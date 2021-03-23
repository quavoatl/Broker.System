using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.System.Installers.CustomMiddleware
{
    public class SignInIntegrationTestUserHelper
    {
        public async Task SignInIntegrationTestUser(HttpContext context)
        {
            var integrationTestsUserHeader = context.Request.Headers["IntegrationTestLogin"];
            if (integrationTestsUserHeader.Count > 0)
            {
                var userName = integrationTestsUserHeader[0];
                var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                var user = await userManager.FindByEmailAsync(userName);
                if (user == null) return;
               
                var signInManager = context.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
                var userIdentity = await signInManager.CreateUserPrincipalAsync(user);
                context.User = userIdentity;
            }
            
        }
    }
}