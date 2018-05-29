using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using IdentityServer.Domain;
using IdentityServer.Core.Shared;
using IdentityServer4.Services;
using IdentityServer4.EntityFramework;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer.Core.Shared.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using IdentityServer.Shared.Abstractions;

namespace IdentityServer.Authentication
{
    public class AuthenticationServices : IAuthentication
    {
        private UserManager<ApplicationUser> _userManager = null;
        private SignInManager<ApplicationUser> _signInManager = null;
        private RoleManager<IdentityRole> _roleManager = null;
        private IIdentityServerInteractionService _interaction = null;
        private IClientStore _clientStore = null;
        private IEventService _eventService = null;
        private void InitializeManagers(IServiceProvider serviceProvider)
        {
            if (_userManager == null || _signInManager == null 
                || _roleManager ==null ||_interaction == null 
                || _clientStore == null || _eventService == null)
            {                 
                _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                _signInManager = serviceProvider.GetService<SignInManager<ApplicationUser>>();
                _roleManager = serviceProvider.GetService <RoleManager<IdentityRole>>();
                _interaction = serviceProvider.GetService<IIdentityServerInteractionService>();
                _clientStore = serviceProvider.GetService<IClientStore>();
                _eventService = serviceProvider.GetService<IEventService>();


            }

        }

        public AuthenticationServices(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _eventService = events;
        }

        public void Configure(IApplicationBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> LoginProcess(string email, string password, bool remember)
        {

            var result = await _signInManager.PasswordSignInAsync(email, password, remember, lockoutOnFailure: false);

            if(!result.Succeeded)
                await _eventService.RaiseAsync(new UserLoginFailureEvent(email, "invalid credentials"));

            return result.Succeeded;
        }

        public async Task LogoutProcess(ClaimsPrincipal User)
        {
            await _signInManager.SignOutAsync();
            await _eventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        public async Task<bool> RegisterProcess(ApplicationUser user, string password, Position position)
        {
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            await _userManager.AddToRoleAsync(user, position.RoleName);
            

            return result.Succeeded;
        }
        public bool IsUserSignedIn(ClaimsPrincipal User)
        {
            var result = _signInManager.IsSignedIn(User);
            return result;
        }




        public void InitializeData(IServiceProvider serviceProvider)
        {
            
            
            if (!_roleManager.RoleExistsAsync(Constants.GeneralManagerRole).Result)
            {
                var role = new IdentityRole(Constants.GeneralManagerRole);
                var result = _roleManager.CreateAsync(role);
            }

            if (!_roleManager.RoleExistsAsync(Constants.DepartmentManagerRole).Result)
            {
                var role = new IdentityRole(Constants.DepartmentManagerRole);
                var result = _roleManager.CreateAsync(role);
                result.Wait();
            }

            if (!_roleManager.RoleExistsAsync(Constants.TeamLeaderRole).Result)
            {
                var role = new IdentityRole(Constants.TeamLeaderRole);
                var result = _roleManager.CreateAsync(role);
                result.Wait();
            }

            if (!_roleManager.RoleExistsAsync(Constants.DeveloperRole).Result)
            {
                var role = new IdentityRole(Constants.DeveloperRole);
                var result = _roleManager.CreateAsync(role);
                result.Wait();
            }

            if (!_roleManager.RoleExistsAsync(Constants.QARole).Result)
            {
                var role = new IdentityRole(Constants.QARole);
                var result = _roleManager.CreateAsync(role);
                result.Wait();
            }

            if (!_roleManager.RoleExistsAsync(Constants.OfficeManagerRole).Result)
            {
                var role = new IdentityRole(Constants.OfficeManagerRole);
                var result = _roleManager.CreateAsync(role);
                result.Wait();
            }


        }

        public void InitializeContext(IServiceCollection services, IConfiguration Configuration)
        {
            //Add auth service
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("IdentityManagerDb"),
                 b => b.MigrationsAssembly("IdentityServer.Authentication")));

            // Add application services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddIdentityServer()
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(Configuration.GetConnectionString("IdentityManagerDb"),
                             sql => sql.MigrationsAssembly("IdentityServer.Authentication"));
                 })
                 .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(Configuration.GetConnectionString("IdentityManagerDb"),
                             sql => sql.MigrationsAssembly("IdentityServer.Authentication"));

                     // this enables automatic token cleanup. this is optional.
                     options.EnableTokenCleanup = true;
                     options.TokenCleanupInterval = 30; // interval in seconds
                 })
                 .AddAspNetIdentity<ApplicationUser>();

            InitializeManagers(services.BuildServiceProvider());
        }

        public async Task<bool> CancelButtonProcessAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
                return false;
            }
            return true;
        }

        public async Task<string> GetAuthorizationContextAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context != null)
                return context?.LoginHint;
            return null;
            
        }

        public async Task<LogoutContext> GetLogoutContextShowSignoutPromptAsync(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);

            if (context != null)
            {
                var logoutContext = new LogoutContext
                {
                    ClientName = context.ClientName,
                    ClientId = context.ClientId,
                    PostLogoutRedirectUri = context.PostLogoutRedirectUri,
                    SignOutIFrameUrl = context.SignOutIFrameUrl
                };
                return logoutContext;
            }
            return null;
            
        }

        public async Task<bool> GetAllowLocalAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context != null)
            {
                var allowLocal = true;
                if (context?.ClientId != null)
                {
                    var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                    if (client != null)
                    {
                        allowLocal = client.EnableLocalLogin;
                    }
                }
                return allowLocal;
            }
            return false;
        }




        public async Task<LogoutContext> GetLogoutContext(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context != null)
            {
                var logoutContext = new LogoutContext
                {
                    ClientName = context.ClientName,
                    ClientId = context.ClientId,
                    PostLogoutRedirectUri = context.PostLogoutRedirectUri,
                    SignOutIFrameUrl = context.SignOutIFrameUrl
                };
                return logoutContext;
            }
            return null;
        }

        public async Task<string> CreateLogoutContextAsync()
        {
            string retVal = null;
            retVal =  await _interaction.CreateLogoutContextAsync();
            return retVal;

        }

        public async Task<string> GetUserRoleByUsernameAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.SingleOrDefault();
        }

        public void UpdateEmployeeName(string name1, string name2)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRoleAsync(string username, string position)
        {
            var oldUser = await _userManager.FindByEmailAsync(username);
            var oldRoles = await _userManager.GetRolesAsync(oldUser);
            var oldRole = oldRoles.SingleOrDefault();
            if (oldRole != null)
            {
                await _userManager.RemoveFromRoleAsync(oldUser, oldRole);
                await _userManager.AddToRoleAsync(oldUser, position);
            }
            else
            {
                await _userManager.AddToRoleAsync(oldUser, position);
            }
        }

    }
}
