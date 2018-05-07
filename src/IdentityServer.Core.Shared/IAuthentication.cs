using IdentityServer.Shared.Abstractions;
using System.Threading.Tasks;

namespace IdentityServer.Core.Shared
{
    public interface IAuthentication : IInitializer
    {
        Task<bool> LogoutProcess();
        Task<bool> LoginProcess(string email, string password, bool remember);
        //Task<bool> RegisterProcess(ApplicationUser user, string password);
        //Task<ApplicationUser> Index(ClaimsPrincipal user);
        //bool IsUserSignedIn(ClaimsPrincipal User);
        //Task<string> GetUserNameAsync(ClaimsPrincipal User);
        //Task<string> GetUserIdAsync(ClaimsPrincipal User);
    }
}
