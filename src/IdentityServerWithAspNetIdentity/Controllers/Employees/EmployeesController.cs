// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer
{
    [Route("[controller]/[action]")]
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

        // GET: Employees/ManageEmployees
        [HttpGet]
        public async Task<IActionResult> ManageEmployees()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                var model = new AllEmployees
                {
                    Employees = new List<Employee>
                    {
                        new Employee
                        {
                            Id = 1,
                            Name = "Ionescu Andrei",
                            Active = true,
                            Department = new Department
                            {
                                Name = "Putere"
                            },
                            Team = new Team
                            {
                                Name = "Fantasticii"
                            }
                        },
                        new Employee
                        {
                            Id = 2,
                            Name = "Marinescu Ionut",
                            Active = false,
                            Department = new Department
                            {
                                Name = "None"
                            },
                            Team = new Team
                            {
                                Name = "None"
                            }
                        },
                        new Employee
                        {
                            Id = 3,
                            Name = "Popescu Florin",
                            Active = true,
                            Department = new Department
                            {
                                Name = "Putere"
                            },
                            Team = new Team
                            {
                                Name = "Spuma Marii"
                            }
                        },
                        new Employee
                        {
                            Id = 4,
                            Name = "Calinescu Robert",
                            Active = true,
                            Department = new Department
                            {
                                Name = "Rupere"
                            },
                            Team = new Team
                            {
                                Name = "Sapa de Lemn"
                            }
                        }
                    }
                };
                return View("ManageEmployees", model);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeInfo/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> EmployeeInfo(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var model = new SingleEmployee
                {
                    Id = 1,
                    Name = "Ionescu Andrei",
                    Active = true,
                    Picture = "8f30cacc4a846a39abc755cb03d748d7_400x400.jpeg",
                    Department = new Department
                    {
                        Name = "Putere"
                    },
                    Team = new Team
                    {
                        Name = "Fantasticii"
                    },
                    Role = "Dev"
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