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
                    Role = employeeRole,
                    CV = employee.CV
                };

                return View("EmployeeInfo", model);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeAdd
        [HttpGet]
        public async Task<IActionResult> EmployeeAdd()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                var model = new AddEmployee()
                {
                    Picture = "abc.jpg",
                    CV = "",
                    Active = true,
                    Role = "",
                    AllRoles = new List<string>
                    {
                        "OM",
                        "GM",
                        "DM",
                        "TL",
                        "QA"
                    },
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Employees/AddEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeAdd(AddEmployee model)
        {
            // Add roles required !!! - delete this when you add 
            // the function to populate the model with roles,
            // in case not all inputs are added
            model.AllRoles = new List<string>
            {
                "OM",
                "GM",
                "DM",
                "TL",
                "QA"
            };
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Username };
                //var result = await _auth.RegisterProcess(user, model.Password);
                //if (result)
                //{
                //if (model.Role == "General Manager")
                //{
                //    _teacherServices.AddTeacher(user);
                //    await _auth.SetUserRole(user, "Teacher");
                //}
                //else if (...)
                //{
                //    _studentServices.AddStudent(user);
                //    await _auth.SetUserRole(user, "Student");
                //}

                //_logger.LogInformation("User created a new account with password.");
                return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                string ErrorMessage = $"the password does not meet the password policy requirements.";
                var policyRequirements = $"* At least an uppercase and a special character";

                ViewBag.Error = ErrorMessage;
                ViewBag.policyRequirments = policyRequirements;
                return View();
                //}
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Employees/EmployeeEdit/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> EmployeeEdit(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var model = new EditEmployee
                {
                    Id = idEmployee,
                    Name = "Ionescu Andrei",
                    Username = "abc@yahoo.com",
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
                    Password = "parola",
                    ConfirmPassword = "parola",
                    AllRoles = new List<string>
                    {
                        "OM",
                        "GM",
                        "DM",
                        "TL",
                        "QA",
                        "Dev"
                    },
                    Role = "QA",
                    CV = "europass.pdf"
                };

                return View(model);
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