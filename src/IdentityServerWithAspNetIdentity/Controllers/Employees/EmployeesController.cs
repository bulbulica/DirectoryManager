// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Core.Shared;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer
{
    [SecurityHeaders]
    public class EmployeesController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        //private readonly IAuthentication _auth;

        public EmployeesController(IIdentityServerInteractionService interaction)
            //,IAuthentication auth)
        {
            _interaction = interaction;
            //_auth = auth;
        }

        public IActionResult ManageEmployees()
        {
            IActionResult retView = null;
            //if (_auth.IsUserSignedIn(User))
            if (1==1)
            {
                retView = View("ManageEmployees");
            }
            else
            {
                retView = NotFound();
            }
            return retView;
        }

        // GET: Employees/Employee/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Employee(int? id)
        {
            IActionResult retView = null;
            //if (_auth.IsUserSignedIn(User))
            if (1 == 1)
            {
                int idEmployee = id ?? default(int);

                var model = new 
                {

                };

                return View("SingleEmployee", model);
            }
            else
            {
                return NotFound();
            }
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