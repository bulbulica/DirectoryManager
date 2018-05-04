using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using StudentsManagement.Domain;
using StudentsManagement.Shared.Abstractions;
using System.Collections.Generic;
//using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentsManagement.Core.Shared
{
    public interface IAuthentication : IInitializer
    {
        Task<bool> LogoutProcess();
        Task<bool> LoginProcess(string email, string password, bool remember);
        Task<bool> RegisterProcess(ApplicationUser user, string password);
        Task<bool> ExternalLoginCallBack();
        Task<bool> ExternalLogicConfirmation(string email);
        AuthenticationProperties ExternalLogin(string provider, string redirectUrl);
        Task<ApplicationUser> Index(ClaimsPrincipal user);
        Task<bool> ProfileUpdate(ClaimsPrincipal claimsPrincipalUser, string modelEmail, string modelPhoneNumber);
        Task<bool> CheckPasswordData(ClaimsPrincipal claimsPrincipalUser);
        Task<bool> ChangePassword(ClaimsPrincipal claimsPrincipalUser, string oldPassword, string newPassword);
        Task<bool> SetPassword(ClaimsPrincipal claimsPrincipalUser, string newPassword);
        Task<bool> IsTeacher(ClaimsPrincipal User);
        Task<bool> IsUserValid(ClaimsPrincipal User);
        bool IsUserSignedIn(ClaimsPrincipal User);
        Task<string> GetUserNameAsync(ClaimsPrincipal User);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task<string> GetUserIdAsync(ClaimsPrincipal User);
        Task SetUserRole(ApplicationUser user, string roleName);
    }
}
