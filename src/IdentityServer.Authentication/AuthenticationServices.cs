using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using StudentsManagement.Domain;
using StudentsManagement.Core.Shared;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentsManagement.Persistence.EF;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Authentication
{
    public class AuthenticationServices : IAuthentication
    {
        private UserManager<ApplicationUser> _userManager = null;
        private SignInManager<ApplicationUser> _signInManager = null;

        private void InitializeManagers(IServiceProvider serviceProvider)
        {
            if (_userManager == null || _signInManager == null)
            {                 
                _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                _signInManager = serviceProvider.GetService<SignInManager<ApplicationUser>>();
            }

        }        

        public AuthenticationServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void Configure(IApplicationBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ExternalLogicConfirmation(string email)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApplicationException("Error loading external login information during confirmation.");
            }
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return true;
                }
                return false;
            }
            return false;
        }

        public AuthenticationProperties ExternalLogin(string provider, string redirectUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return properties;
        }

        public async Task<bool> ExternalLoginCallBack()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return false;
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            return result.Succeeded;
        }


        public async Task<bool> LoginProcess(string email, string password, bool remember)
        {

            var result = await _signInManager.PasswordSignInAsync(email, password, remember, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<bool> LogoutProcess()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<bool> RegisterProcess(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        public async Task<ApplicationUser> Index(ClaimsPrincipal claimsPrincipalUser)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipalUser);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(claimsPrincipalUser)}'.");
            }

            return user;
        }

        public async Task<bool> ProfileUpdate(ClaimsPrincipal claimsPrincipalUser, string modelEmail, string modelPhoneNumber)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipalUser);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(claimsPrincipalUser)}'.");
            }

            var email = user.Email;
            if (modelEmail != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, modelEmail);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (modelPhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, modelPhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }
            return true;
        }

        public async Task<bool> CheckPasswordData(ClaimsPrincipal claimsPrincipalUser)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipalUser);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(claimsPrincipalUser)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            return hasPassword;
        }

        public async Task<bool> ChangePassword(ClaimsPrincipal claimsPrincipalUser, string oldPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipalUser);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(claimsPrincipalUser)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changePasswordResult.Succeeded)
            {
                return false;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }

        public async Task<bool> SetPassword(ClaimsPrincipal claimsPrincipalUser, string newPassword)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipalUser);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(claimsPrincipalUser)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addPasswordResult.Succeeded)
            {
                return false;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }

        public async Task<bool> IsTeacher(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID ");
            }
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if (role == "Teacher")
                    return true;
            }
            return false;
            
        }

        public async Task<bool> IsUserValid(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return false;
            else
                return true;
            
        }

        public bool IsUserSignedIn(ClaimsPrincipal User)
        {
            var result = _signInManager.IsSignedIn(User);
            return result;
        }

        public void InitializeContext(IServiceCollection services, IConfiguration Configuration)
        {
            //Add auth service
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("WebStudentsManagement")));
            // Add application services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            InitializeManagers(services.BuildServiceProvider());
        }


        public void InitializeData(IServiceProvider serviceProvider)
        {
           
        }

        public async Task<string> GetUserNameAsync(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            return user.UserName;
        }

        public Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return _signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public async Task<string> GetUserIdAsync(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            return user.Id;
        }

        public async Task SetUserRole(ApplicationUser user, string roleName)
        {
           var result = await _userManager.AddToRoleAsync(user, roleName);
            Console.WriteLine(result.Errors);
        }
    }
}
