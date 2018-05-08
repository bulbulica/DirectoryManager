using IdentityServer.Core.Shared.Models;
using IdentityServer.Shared.Abstractions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Core.Shared
{
    public interface IAuthentication : IInitializer
    {
        Task<bool> LoginProcess(string email, string password, bool remember);
        //RegisterProcess(ApplicationUser user, string password);
        //Task<ApplicationUser> Index(ClaimsPrincipal user);
        bool IsUserSignedIn(ClaimsPrincipal User);
        Task<bool> CancelButtonProcessAsync(string returnUrl);
        Task LogoutProcess(ClaimsPrincipal user);
        Task<string> GetAuthorizationContextAsync(string returnUrl);
        Task<bool> GetLogoutContextShowSignoutPromptAsync(string logoutId);
        bool GetAllowLocalAsync(string returnUrl);
        Task<LogoutContext> GetLogoutContext(string returnUrl);
        Task<string> CreateLogoutContextAsync();
        //Task<string> GetUserNameAsync(ClaimsPrincipal User);
        //Task<string> GetUserIdAsync(ClaimsPrincipal User);
    }
}
