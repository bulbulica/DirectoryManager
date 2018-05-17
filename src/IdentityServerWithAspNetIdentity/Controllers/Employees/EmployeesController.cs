// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> EmployeeAddCV(int? id, IFormFile file)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);
                var employee = _employeeService.GetEmployee(idEmployee);

                if (Path.GetExtension(file.FileName).ToLower() != ".pdf"
               && Path.GetExtension(file.FileName).ToLower() != ".doc"
               && Path.GetExtension(file.FileName).ToLower() != ".docx")
                {
                    //Temporar pana modificam view-ul
                    return NotFound();
                }

                if (file.Length > 10000000)
                {
                    return NotFound();
                }
               
                //HARD CODED filePath
               //TODO
                var directoryPath = Path.Combine("wwwroot", "uploads");
                directoryPath = Path.Combine(directoryPath, "CV");
                //TODO
                //folosim doar Name pentru salvare CV, temporar, ar fi ok Nume + prenume dar nu le introducem la register
                var filePath = Path.Combine(directoryPath, employee.Name);
                filePath = string.Concat(filePath, Path.GetExtension(file.FileName).ToLower());
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                if (employee != null)
                {
                    _employeeService.UpdateCV(employee, filePath);
                }
                return RedirectToAction(nameof(ManageEmployees));
            }
        }

        // GET: Employees/EmployeeAdd
        [HttpGet]
        public IActionResult EmployeeAdd()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == "email");

                var model = new AddEmployee()
                {
                    Active = true,
                    //AllPositions = _employeeService.GetRegisterPositionsByAccessLevel(username.ToString()),
                    //folosim codul de mai sus dupa ce suntem logati, deocamdata o sa dea crash
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

                if (ModelState.IsValid)
                {
                    var position = _employeeService.GetPositionByName(model.Position);
                    var department = _employeeService.GetDepartmentByName(model.Department);

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
                                CV = "" // am modificat si aici !!!
                            };
                            _employeeService.AddEmployee(employee);
                            if (employee.Position == _employeeService.GetDepartmentManagerPosition())
                            {
                                _employeeService.UpdateDepartmentManager(employee.Department, employee);
                            }

                        }
                        else
                        {
                            var ErrorMessage = $"the password does not meet the password policy requirements.";
                            var policyRequirements = $"* At least an uppercase and a special character";

                            ViewBag.Error = ErrorMessage;
                            ViewBag.policyRequirments = policyRequirements;

                            var returnModel = new AddEmployee()
                            {
                                Active = true,
                                //AllPositions = _employeeService.GetRegisterPositionsByAccessLevel(username.ToString()),
                                //folosim codul de mai sus dupa ce suntem logati, deocamdata o sa dea crash
                                AllPositions = _employeeService.GetAllPositions(),
                                AllDepartments = _employeeService.GetAllDepartments()
                            };

                            return View(returnModel);
                        }
                    }
                }

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
                    Picture = "", // am modificat si aici !!!
                    Team = employee.Team,
                    CV = "", // am modificat si aici !!!
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
                    var employee = _employeeService.GetEmployee(idEmployee);

                    if (position != null && department != null)
                    {
                        employee.Name = model.Name;
                        employee.Active = true;
                        employee.Position = position;
                        employee.Department = department;
                        employee.CV = model.CV;
                    };

                    _employeeService.UpdateEmployee(employee);

                    if (employee.Position == _employeeService.GetDepartmentManagerPosition())
                    {
                        _employeeService.UpdateDepartmentManager(employee.Department, employee);
                    }

                    else if (employee.Position == _employeeService.GetTeamLeaderPosition())
                    {
                        if (employee.Team != null)
                        {
                            if (_employeeService.GetTeamLeader(employee.Team) != employee)
                            {
                                _employeeService.UpdateTeamLeader(employee.Team, employee);
                            }
                        }
                    }
                }
                else
                {
                    string ErrorMessage = $"the password does not meet the password policy requirements.";
                    var policyRequirements = $"* At least an uppercase and a special character";

                    ViewBag.Error = ErrorMessage;
                    ViewBag.policyRequirments = policyRequirements;
                    return View("EmployeeEdit", model);
                }

                return ManageEmployees();
            }
            else
            {
                return NotFound();
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

    }
}