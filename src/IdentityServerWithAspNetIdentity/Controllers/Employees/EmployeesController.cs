﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer
{
    [Route("[controller]/[action]")]
    [SecurityHeaders]
    [Authorize]
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
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel < 3)
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

        [HttpGet("{departmentId}")]
        public IActionResult ManageEmployeesForDepartmentManager(int departmentId)
        {
            var department = _employeeService.GetDepartment(departmentId);
            List<Employee> employees = _employeeService.GetAllEmployeesFromDepartment(department);

            var model = new AllEmployees
            {
                Employees = employees
            };
            return View(model);
        }

        [HttpGet("{username}")]
        public IActionResult GetManageEmployees(string username)
        {
            if(username != User.Identity.Name)
            {
                return NotFound();
            }

            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel == 2)
            {
                return RedirectToAction("ManageEmployeesForDepartmentManager", new { departmentId = user.Department.Id });
            }
            else if (user.Position.AccessLevel == 1)
            {
                return RedirectToAction(nameof(ManageEmployees));
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeInfo/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeInfo(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.AccessLevel < employee.Position.AccessLevel
                ||user.Id ==employee.Id)
            {
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View(model);
            }
            
            else if(user.Name == employee.Username)
            { 
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

        // GET: Employees/EmployeeInfoFromTeamLeader/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromTeamLeader(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if(user.Position.RoleName != Constants.TeamLeaderRole)
            {
                return RedirectToAction("EmployeeInfo", idEmployee);
            }

            if (user.Position.AccessLevel < employee.Position.AccessLevel
                || user.Id == employee.Id)
            {
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View(model);
            }

            else if (user.Name == employee.Username)
            {
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

        // GET: Employees/EmployeeInfoFromDepartmentManager/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromDepartmentManager(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.TeamLeaderRole)
            {
                return RedirectToAction("EmployeeInfo", idEmployee);
            }

            if (user.Position.AccessLevel < employee.Position.AccessLevel
                || user.Id == employee.Id)
            {
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View(model);
            }

            else if (user.Name == employee.Username)
            {
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

        // GET: Employees/EmployeeInfoFromGeneralManager/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromGeneralManager(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.TeamLeaderRole)
            {
                return RedirectToAction("EmployeeInfo", idEmployee);
            }

            if (user.Position.AccessLevel < employee.Position.AccessLevel
                || user.Id == employee.Id)
            {
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View(model);
            }

            else if (user.Name == employee.Username)
            {
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

        // GET: Employees/EmployeeInfoFromOfficeManager/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromOfficeManager(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.TeamLeaderRole)
            {
                return RedirectToAction("EmployeeInfo", idEmployee);
            }

            if (user.Position.AccessLevel < employee.Position.AccessLevel
                || user.Id == employee.Id)
            {
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View(model);
            }

            else if (user.Name == employee.Username)
            {
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
        [HttpGet("{id}")]
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
        [HttpPost("{id}")]
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
                    var error = "Not a valid format: .pdf, .doc or .docx)";

                    var model = new AddCVEmployee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Error = error
                    };

                    return View(model);
                }

                if (file.Length > 10000000)
                {
                    var error = "File exceeded size limit";

                    var model = new AddCVEmployee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Error = error
                    };

                    return View(model);
                }
               
                var fileName = employee.Username;
                var directoryPath = Path.Combine("wwwroot", "docs");
                var filePath = Path.Combine(directoryPath, fileName);
                fileName = string.Concat(fileName, Path.GetExtension(file.FileName).ToLower());
                filePath = string.Concat(filePath, Path.GetExtension(file.FileName).ToLower());
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                if (employee != null)
                {
                    _employeeService.UpdateCV(employee, fileName);
                }

                return RedirectToAction("EmployeeInfo", idEmployee);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/GetEmployeeInfo/{username}
        [HttpGet("{username}")]
        public IActionResult GetEmployeeInfo(string username)
        {
            var user = _employeeService.GetEmployeeByName(username);
            if (user != null)
            {
                return RedirectToAction("EmployeeInfo", new { id = user.Id });
            }
            return NotFound();
        }


        // GET: Employees/EmployeeChangePosition/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeChangePosition(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.AccessLevel < _employeeService.GetDeveloperPosition().AccessLevel)
            {
                var positions = _employeeService.GetRegisterPositionsByAccessLevel(user.Username);
                var model = new EditEmployee
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    AllPositions = positions,
                    Position = employee.Position.RoleName
                    
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeChangePosition(int? id, EditEmployee model)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);
            var position = _employeeService.GetPositionByName(model.Position);
            if (user.Position.AccessLevel > employee.Position.AccessLevel
               || user.Position.AccessLevel > position.AccessLevel)
            {
                return NotFound();
            }

            if (position == _employeeService.GetDepartmentManagerPosition()
                && employee.Department != null)
            {
                _employeeService.UpdateDepartmentManager(employee.Department, employee);
            }

            else if (position == _employeeService.GetTeamLeaderPosition()
                && employee.Team != null)
            {
                _employeeService.UpdateTeamLeader(employee.Team,employee);
            }
            _employeeService.UpdateEmployeePosition(position, employee);
                return RedirectToAction("EmployeeInfoFromTeamLeader", idEmployee);
            //Adaugare warnings/errors in caz ca nu se updateaza
        }


        // GET: Employees/EmployeeAddImage/{id}
        [HttpGet("{id}")]
        public IActionResult EmployeeAddImage(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var employee = _employeeService.GetEmployee(idEmployee);
                var model = new AddImageEmployee
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

        // POST: Employees/EmployeeAddImage/{id}
        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeAddImage(int? id, IFormFile file)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);
                var employee = _employeeService.GetEmployee(idEmployee);

                if (Path.GetExtension(file.FileName).ToLower() != ".jpeg"
                    && Path.GetExtension(file.FileName).ToLower() != ".jpg"
                    && Path.GetExtension(file.FileName).ToLower() != ".png")
                {
                    var error = "Not a valid format: .jpeg, .jpg or .png)";

                    var model = new AddImageEmployee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Error = error
                    };

                    return View(model);
                }

                if (file.Length > 5000000)
                {
                    var error = "File exceeded size limit";

                    var model = new AddImageEmployee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Error = error
                    };

                    return View(model);
                }

                var fileName = employee.Username;
                var directoryPath = Path.Combine("wwwroot", "img");
                var filePath = Path.Combine(directoryPath, fileName);
                fileName = string.Concat(fileName, Path.GetExtension(file.FileName).ToLower());
                filePath = string.Concat(filePath, Path.GetExtension(file.FileName).ToLower());
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                if (employee != null)
                {
                    _employeeService.UpdateImage(employee, fileName);
                }

                return RedirectToAction("EmployeeInfo", idEmployee);
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
                var username = User.Identity.Name;

                var model = new AddEmployee()
                {
                    Active = true,
                    AllPositions = _employeeService.GetRegisterPositionsByAccessLevel(username.ToString()),
                    AllDepartments = _employeeService.GetAllDepartments()
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Employees/EmployeeAdd
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
                    var exDepartmentManager = _employeeService.GetDepartmentManager(department);

                    if (position != null && department != null)
                    {
                        var user = new ApplicationUser { UserName = model.Username, FullName = model.Name, Email = model.Username };
                        var result = await _auth.RegisterProcess(user, model.Password, position);
                        if (result)
                        {
                            var employee = new Employee
                            {
                                Name = model.Name,
                                Username = model.Username,
                                Active = true,
                                Position = position,
                                Department = department,
                            };
                            _employeeService.AddEmployee(employee);
                            if (employee.Position == _employeeService.GetDepartmentManagerPosition())
                            {
                                if (exDepartmentManager != null)
                                {
                                    await _auth.UpdatePositionAsync(exDepartmentManager, Constants.DeveloperRole);
                                }
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
                                AllPositions = _employeeService.GetRegisterPositionsByAccessLevel(User.Identity.Name),
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
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idEmployee = id ?? default(int);

            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel) {
                var employee = _employeeService.GetEmployee(idEmployee);
                var positions = _employeeService.GetRegisterPositionsByAccessLevel(User.Identity.Name);
                var departments = _employeeService.GetAllDepartments();
                var model = new EditEmployee
                {
                    Id = idEmployee,
                    Name = employee.Name,
                    AllPositions = positions,
                    AllDepartments = departments
                    
                };

                return View(model);
            }
            else
            {
                var employee = _employeeService.GetEmployee(idEmployee);
                var localUser = _employeeService.GetEmployeeByName(User.Identity.Name);
                var positions = _employeeService.GetRegisterPositionsByAccessLevel(User.Identity.Name);
                if (localUser.Id == employee.Id)
                {
                    var model = new 
                    {
                    };
                    return View("EmployeeEditHimself", model);
                }
            }   
            return NotFound();
        }

        // POST: Employees/EmployeeEdit/{id}
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


        // GET: Employees/EmployeeEditName/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeEditName(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idEmployee = id ?? default(int);

                var employee = _employeeService.GetEmployee(idEmployee);

                var model = new EditEmployeeName
                {
                    Id = idEmployee,
                    Name = employee.Name
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Employees/EmployeeEditName/{id}
        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeEditName(EditEmployeeName model, int? id)
        {
            int idEmployee = id ?? default(int);

            if (ModelState.IsValid)
            {
                var employee = _employeeService.GetEmployee(idEmployee);
                if (model.Name.Length > 6)
                {
                    _employeeService.UpdateEmployeeName(employee, model.Name);
                }
                //Add Error 

            return RedirectToAction("EmployeeInfo", idEmployee);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Employees/EmployeeDelete/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeDelete(int? id)
        {
            //TODO 
            //check accessLevel before 
            if (true)
            {
                int idEmployee = id ?? default(int);
                _employeeService.DeleteEmployee(idEmployee);
                return RedirectToAction(nameof(ManageEmployees));
            }
            else
            {
                return NotFound();
            }
        }
    }
}