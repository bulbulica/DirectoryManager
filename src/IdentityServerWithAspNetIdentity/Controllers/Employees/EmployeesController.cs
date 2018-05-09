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
        private readonly IAuthentication _auth;
        private readonly IBusinessLayer _businessLogic;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IIdentityServerInteractionService interaction
            ,IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _interaction = interaction;
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
        }

        // GET: Employees/ManageEmployees
        [HttpGet]
        public IActionResult ManageEmployees()
        {
            //if (_auth.IsUserSignedIn(User))
            //{
                List<Employee> employees = _employeeService.GetAllEmployees();

                var model = new AllEmployees
                {
                    Employees = employees
                };
                return View("ManageEmployees", model);

            //}
            //else
            //{
            //    return NotFound();
            //}
        }

        // GET: Employees/EmployeeInfo/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeInfo(int? id)
        {
            if (_auth.IsUserSignedIn(User))
            {
                int idEmployee = id ?? default(int);

                var employee = _employeeService.GetEmployee(idEmployee);

                var employeeRole = "";
                if (employee.Position != null)
                    employeeRole = employee.Position.RoleName;


                var model = new SingleEmployee
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Active = employee.Active,
                    Picture = employee.Picture,
                    Department = employee.Department,
                    Team = employee.Team,
                    Role = employeeRole
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