// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Core.Shared;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthentication _auth;

        public HomeController(IIdentityServerInteractionService interaction,
            IAuthentication auth)
        {
            _interaction = interaction;
            _auth = auth;
        }

        public IActionResult Index()
        {
            IActionResult retView = null;
            if (_auth.IsUserSignedIn(User))
            {
                retView = View("Index");
            }
            else
            {
                retView = RedirectToAction("Login", "Account");
            }
            return retView;
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}