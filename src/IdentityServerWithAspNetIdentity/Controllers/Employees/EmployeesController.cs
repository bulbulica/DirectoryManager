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
        private readonly IAuthentication _auth;
        private readonly IBusinessLayer _businessLogic;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
        }

        // GET: Employees/ManageEmployees
        [HttpGet]
        public IActionResult ManageEmployees()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                List<Employee> employees = _employeeService.GetAllEmployees();

                var model = new AllEmployees
                {
                    Employees = employees
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
        public IActionResult EmployeeInfo(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var employee = _employeeService.GetEmployee(idEmployee);
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeAddCV/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeAddCV(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var employee = _employeeService.GetEmployee(idEmployee);
                var model = new AddCVEmployee
                {
                    Id = employee.Id,
                    Name = employee.Name
                };
                
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Employees/EmployeeAddCV/{id}
        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeAddCV(AddCVEmployee model)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            { 
                // AICI TREBUIE ADAUGAT PENTRU CV
                return ManageEmployees();
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeAdd
        [HttpGet]
        public IActionResult EmployeeAdd()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                var model = new AddEmployee()
                {
                    Active = true,
                    AllPositions = _employeeService.GetAllPositions(),
                    AllDepartments = _employeeService.GetAllDepartments()
                    
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
        public async Task<IActionResult> EmployeeAdd(AddEmployee model)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                // Add roles required !!! - delete this when you add 
                // the function to populate the model with roles,
                // in case not all inputs are added
                model.AllPositions = _employeeService.GetAllPositions();

                if (model.CV == null)
                    model.CV = "";
                if (model.Picture == null)
                    model.Picture = "";

                if (ModelState.IsValid)
                {
                    var position = _employeeService.GetPositionByName(model.Position);
                    var department = _employeeService.GetDepartmentByName(model.Department);


                    //TODO check if position is departmentmanager and if that department already has a manager.
                    //if so update


                    if (position != null && department != null)
                    {
                        var user = new ApplicationUser { UserName = model.Name, Email = model.Username };
                        var result = await _auth.RegisterProcess(user, model.Password);
                        if (result)
                        {
                            var employee = new Employee
                            {
                                Name = model.Name,
                                Username = model.Username,
                                Active = true,
                                Position = position,
                                Department = department,
                                CV = model.CV
                            };
                            _employeeService.AddEmployee(employee);

                        }
                    }

                    else
                    {
                        string ErrorMessage = $"the password does not meet the password policy requirements.";
                        var policyRequirements = $"* At least an uppercase and a special character";

                        ViewBag.Error = ErrorMessage;
                        ViewBag.policyRequirments = policyRequirements;
                        return View();
                    }
                }

                // If we got this far, something failed, redisplay form
                return ManageEmployees();
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeEdit/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeEdit(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var employee = _employeeService.GetEmployee(idEmployee);

                var model = new EditEmployee
                {
                    Id = idEmployee,
                    Name = employee.Name,
                    Active = employee.Active,
                    AllPositions = _employeeService.GetAllPositions(),
                    Department = employee.Department.Name,
                    Picture = employee.Picture,
                    Team = employee.Team,
                    CV = employee.CV,
                    Position = employee.Position.RoleName,
                    AllDepartments = _employeeService.GetAllDepartments()
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeEdit/{id}
        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeEdit(EditEmployee model, int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                if (model.CV == null)
                    model.CV = "";
                if (model.Picture == null)
                    model.Picture = "";
                int idEmployee = id ?? default(int);


                if (ModelState.IsValid)
                {
                    var position = _employeeService.GetPositionByName(model.Position);
                    var department = _employeeService.GetDepartmentByName(model.Department);

                    if (position != null && department != null)
                    {
                        var employee = new Employee
                        {
                            Id = idEmployee,
                            Name = model.Name,
                            Active = true,
                            Position = position,
                            Department = department,
                            CV = model.CV

                        };
                        _employeeService.UpdateEmployee(employee);
                    }

                    else
                    {
                        string ErrorMessage = $"the password does not meet the password policy requirements.";
                        var policyRequirements = $"* At least an uppercase and a special character";

                        ViewBag.Error = ErrorMessage;
                        ViewBag.policyRequirments = policyRequirements;
                        return View("EmployeeEdit", model);
                    }
                }
                return ManageEmployees();
            }
            else
            {
                return Forbid();
            }
        }

        // POST: Employees/EmployeeDelete/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeDelete(int? id)
        {
            //TODO 
            //check accessLevel before 

            int idEmployee = id ?? default(int);
            _employeeService.DeleteEmployee(idEmployee);
            return RedirectToAction(nameof(ManageEmployees));
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public IActionResult Error(string errorId)
        {
            var vm = new ErrorViewModel();

            return View("Error", vm);
        }
    }
}