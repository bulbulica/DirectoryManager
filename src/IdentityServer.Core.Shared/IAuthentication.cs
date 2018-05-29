using IdentityServer.Core.Shared.Models;
using IdentityServer.Domain;
using IdentityServer.Shared.Abstractions;
using System.Collections.Generic;
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
        Task<LogoutContext> GetLogoutContextShowSignoutPromptAsync(string logoutId);
        Task<bool> GetAllowLocalAsync(string returnUrl);
        Task<LogoutContext> GetLogoutContext(string logoutId);
        Task<string> CreateLogoutContextAsync();
        Task<string> GetUserRoleByUsernameAsync(string email);
        Task<bool> RegisterProcess(ApplicationUser user, string password, Position position);
        void UpdateEmployeeName(string name1, string name2);
        Task UpdateRoleAsync(string username, string position);
        Task ChangeUserPassword(string username, string password);

        //Task<string> GetUserNameAsync(ClaimsPrincipal User);
        //Task<string> GetUserIdAsync(ClaimsPrincipal User);
    }
}
