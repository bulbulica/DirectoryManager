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
        private readonly IDepartmentService _departmentService;
        private readonly ITeamService _teamService;

        public EmployeesController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
            _departmentService = _businessLogic.GetDepartmentService();
            _teamService = _businessLogic.GetTeamService();
        }

        [HttpGet]
        public IActionResult ManageEmployees()
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);

            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel)
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
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.AccessLevel != Constants.DepartmentManagerAccessLevel)
            {
                return NotFound();
            }

            var department = _departmentService.GetDepartment(departmentId);
            List<Employee> employees = _departmentService.GetAllEmployeesFromDepartment(department);

            var model = new AllEmployees
            {
                Employees = employees
            };
            return View(model);
        }

        public IActionResult ManageEmployeesForGeneralManager()
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);
            if (user.Position.AccessLevel != Constants.GeneralManagerAccessLevel)
            {
                return NotFound();
            }

            List<Employee> employees = _employeeService.GetAllEmployees();
            List<Employee> availableEmployees = new List<Employee>();
            foreach (var employee in employees)
            {
                if (employee.Position.AccessLevel != Constants.OfficeManagerAccessLevel)
                    availableEmployees.Add(employee);
            }


            var model = new AllEmployees
            {
                Employees = availableEmployees
            };
            return View(model);
        }

        public IActionResult ManageEmployeesForOfficeManager()
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);
            if (user.Position.AccessLevel != Constants.OfficeManagerAccessLevel)
            {
                return NotFound();
            }

            List<Employee> employees = _employeeService.GetAllEmployees();

            var model = new AllEmployees
            {
                Employees = employees
            };
            return View(model);
        }

        [HttpGet("{username}")]
        public IActionResult GetManageEmployees(string username)
        {
            if (username != User.Identity.Name)
            {
                return NotFound();
            }

            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
            {
                return RedirectToAction("ManageEmployeesForDepartmentManager", new { departmentId = user.Department.Id });
            }
            else if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                return RedirectToAction(nameof(ManageEmployeesForGeneralManager));
            }
            else if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                return RedirectToAction(nameof(ManageEmployeesForOfficeManager));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult EmployeeInfo(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                var model = new SingleEmployee
                {
                    Employee = employee
                };

                return View("EmployeeInfoHimself", model);
            }
            else
            {
                if (employee == null)
                {
                    return NotFound();
                }

                if (user.Position.AccessLevel < employee.Position.AccessLevel
                    && employee.Position.AccessLevel != Constants.OfficeManagerAccessLevel)
                {
                    var model = new SingleEmployee
                    {
                        Employee = employee
                    };

                    return View(model);
                }
                else if (user.Username == employee.Username)
                {
                    var model = new SingleEmployee
                    {
                        Employee = employee
                    };

                    return View("EmployeeInfoHimself", model);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromTeamLeader(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.TeamLeaderRole)
            {
                return RedirectToAction("EmployeeInfo", idEmployee);
            }

            if (employee == null)
            {
                return NotFound();
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
            else if (user.Username == employee.Username)
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

        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromDepartmentManager(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.DepartmentManagerRole)
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

            else if (user.Username == employee.Username)
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

        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromGeneralManager(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.GeneralManagerRole)
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
            else if (user.Username == employee.Username)
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

        [HttpGet("{id}")]
        public IActionResult EmployeeInfoFromOfficeManager(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.RoleName != Constants.OfficeManagerRole)
            {
                return RedirectToAction("EmployeeInfo", idEmployee);
            }
            var model = new SingleEmployee
            {
                Employee = employee
            };
            return View(model);
        }


        [HttpGet("{id}")]
        public IActionResult EmployeeAddCV(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);

            if (employee == null)
            {
                return NotFound();
            }

            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Username == employee.Username
                || user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
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

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeAddCV(int? id, IFormFile file)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Username == employee.Username
                || user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {

                if (file == null)
                {
                    var error = "CV file cannot be empty";
                    var model = new AddCVEmployee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Error = error
                    };
                    return View(model);
                }

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

        [HttpGet("{id}")]
        public IActionResult EmployeeChangePosition(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (employee == null)
            {
                return NotFound();
            }

            if (user.Position.AccessLevel < _employeeService.GetDeveloperPosition().AccessLevel)
            {
                var positions = _employeeService.GetRegisterPositionsByAccessLevel(user.Username).ToList();
                if (employee.Team == null)
                {
                    positions.Remove(_teamService.GetTeamLeaderPosition());
                }
                if (employee.Department == null)
                {
                    positions.Remove(_departmentService.GetDepartmentManagerPosition());
                }
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
        public async Task<IActionResult> EmployeeChangePosition(int? id, EditEmployee model)
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

            if (position == _departmentService.GetDepartmentManagerPosition()
                && employee.Department != null)
            {
                var exDepartmentManager = _departmentService.GetDepartmentManager(employee.Department);

                if (exDepartmentManager != null)
                {
                    await _auth.UpdateRoleAsync(exDepartmentManager.Username, Constants.DeveloperRole);
                }
                _departmentService.UpdateDepartmentManager(employee.Department, employee);
                await _auth.UpdateRoleAsync(employee.Username, Constants.DepartmentManagerRole);
            }
            else if (position == _teamService.GetTeamLeaderPosition()
                && employee.Team != null)
            {
                var exTeamLeader = _departmentService.GetDepartmentManager(employee.Department);

                if (exTeamLeader != null)
                {
                    await _auth.UpdateRoleAsync(exTeamLeader.Username, Constants.DeveloperRole);
                }
                _teamService.UpdateTeamLeader(employee.Team, employee);
                await _auth.UpdateRoleAsync(employee.Username, Constants.TeamLeaderRole);
            }
            else
            {
                _employeeService.UpdateEmployeePosition(position, employee);
                await _auth.UpdateRoleAsync(employee.Username, position.RoleName);
            }
            if (user.Position.AccessLevel == Constants.TeamLeaderAccessLevel)
            {
                return RedirectToAction("EmployeeInfoFromTeamLeader", idEmployee);
            }
            else if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
            {
                return RedirectToAction("EmployeeInfoFromDepartmentManager", idEmployee);
            }
            else if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                return RedirectToAction("EmployeeInfoFromGeneralManager", idEmployee);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult EmployeeAddImage(int? id)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);

            if (employee == null)
            {
                return NotFound();
            }

            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            string error = null;

            if (ViewBag.Error != null) {
                error = ViewBag.Error;
            }

            if (user.Username == employee.Username
                || user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                var model = new AddImageEmployee
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Error = error
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeAddImage(int? id, IFormFile file)
        {
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Username == employee.Username
                || user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                if (file == null)
                {
                    var error = "You must select an image!";
                    var model = new AddImageEmployee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Error = error
                    };
                    return View(model);
                }

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

        [HttpGet]
        public IActionResult EmployeeAdd()
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            var allPositions = _employeeService.GetAllRegisterPositions();

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                var username = User.Identity.Name;

                var model = new AddEmployee()
                {
                    Active = true,
                    AllPositions = allPositions,
                    AllDepartments = _departmentService.GetAllDepartments()
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeAdd(AddEmployee model)
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                if (ModelState.IsValid)
                {
                    var position = _employeeService.GetPositionByName(model.Position);

                    Department department;
                    if (model.Department == "null")
                    {
                        department = null;
                    }
                    else
                    {
                        department = _departmentService.GetDepartmentByName(model.Department);
                    }

                    if (position.AccessLevel <= Constants.GeneralManagerAccessLevel
                        || position.AccessLevel == Constants.OfficeManagerAccessLevel)
                    {
                        department = null;
                    }

                    if(position.AccessLevel == Constants.DepartmentManagerAccessLevel && department == null)
                    {
                        var ErrorMessage = $"You can't create a Department Manager without a department!.";
                        ViewBag.Error = ErrorMessage;

                        var returnModel = new AddEmployee()
                        {
                            Active = true,
                            AllPositions = _employeeService.GetAllRegisterPositions(),
                            AllDepartments = _departmentService.GetAllDepartments()
                        };
                        return View(returnModel);
                    }

                    if (position != null)
                    {
                        var User = new ApplicationUser { UserName = model.Username, FullName = model.Name, Email = model.Username };
                        var result = await _auth.RegisterProcess(User, model.Password, position);

                        if (result)
                        {
                            var employee = new Employee
                            {                                
                                Name = model.Name,
                                Username = model.Username,
                                Active = true,
                                PositionId = position.Id,
                                Department = department,
                            };
                            
                            _employeeService.AddEmployee(employee);

                            if (position.RoleName.Equals(Constants.DepartmentManagerRole) && department != null)
                            {
                                var exDepartmentManager = _departmentService.GetDepartmentManager(department);

                                if (exDepartmentManager != null)
                                {
                                    await _auth.UpdateRoleAsync(exDepartmentManager.Username, Constants.DeveloperRole);
                                }
                                _departmentService.UpdateDepartmentManager(department, employee);
                            }

                        }
                        else
                        {
                            var ErrorMessage = $"the password does not meet the password policy requirements or the email already exists";
                            ViewBag.Error = ErrorMessage;

                            var returnModel = new AddEmployee()
                            {
                                Active = true,
                                AllPositions = _employeeService.GetAllRegisterPositions(),
                                AllDepartments = _departmentService.GetAllDepartments()
                            };

                            return View(returnModel);
                        }
                    }
                }

                return GetManageEmployees(User.Identity.Name);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeEdit(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idEmployee = id ?? default(int);

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel) {
                var employee = _employeeService.GetEmployee(idEmployee);

                var model = new EditEmployeePassword
                {
                    Id = idEmployee,
                    Name = employee.Name,
                    Username = employee.Username
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

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeEdit(EditEmployee model, int? id)
        {
            if (_auth.IsUserSignedIn(User))
            {
                if (model.CV == null)
                    model.CV = "";
                if (model.Picture == null)
                    model.Picture = "";
                int idEmployee = id ?? default(int);

                if (ModelState.IsValid)
                {
                    var position = _employeeService.GetPositionByName(model.Position);
                    var department = _departmentService.GetDepartmentByName(model.Department);
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

                    if (employee.Position == _departmentService.GetDepartmentManagerPosition())
                    {
                        _departmentService.UpdateDepartmentManager(employee.Department, employee);
                    }
                    else if (employee.Position == _teamService.GetTeamLeaderPosition())
                    {
                        if (employee.Team != null)
                        {
                            if (_teamService.GetTeamLeader(employee.Team) != employee)
                            {
                                _teamService.UpdateTeamLeader(employee.Team, employee);
                            }
                        }
                    }
                }
                else
                {
                    string ErrorMessage = $"the password does not meet the password policy requirements, rr there is already a user with this name in Database";
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

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeChangePassword(EditEmployeePassword model, int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    _auth.ChangeUserPassword(employee.Username, model.Password);
                    return RedirectToAction("ManageEmployeesForOfficeManager");
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeEditName(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);

            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);

            if (employee == null)
            {
                return NotFound();
            }

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel
                || user.Id == employee.Id)
            {

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

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeEditName(EditEmployeeName model, int? id)
        {
            int idEmployee = id ?? default(int);
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var employee = _employeeService.GetEmployee(idEmployee);


            if (ModelState.IsValid 
                && (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel || user.Id == employee.Id))
            {
                if (model.Name.Length > 6)
                {
                    _employeeService.UpdateEmployeeName(employee, model.Name);
                }

                return RedirectToAction("EmployeeInfo", idEmployee);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeDelete(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel) 
            {
                int idEmployee = id ?? default(int);
                _employeeService.DeleteEmployee(idEmployee);

                return RedirectToAction(nameof(ManageEmployeesForOfficeManager));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult RemoveEmployeeFromDepartment(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                if(employee.Position.RoleName == Constants.TeamLeaderRole
                    || employee.Position.RoleName == Constants.DepartmentManagerRole) {
                    _auth.UpdateRoleAsync(employee.Name, Constants.DeveloperRole);
                    _employeeService.UpdateEmployeePosition(_employeeService.GetDeveloperPosition(), employee);
                    }
                _departmentService.RemoveEmployeeFromDepartment(idEmployee);

                return RedirectToAction(nameof(ManageEmployeesForGeneralManager));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult RemoveEmployeeFromTeam(int? id)
        {
            var username = User.Identity.Name;
            int idEmployee = id ?? default(int);
            var employee = _employeeService.GetEmployee(idEmployee);

            var user = _employeeService.GetEmployeeByName(username);

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                if (employee.Position.RoleName == Constants.TeamLeaderRole){
                    _auth.UpdateRoleAsync(employee.Name, Constants.DeveloperRole);
                    _employeeService.UpdateEmployeePosition(_employeeService.GetDeveloperPosition(), employee);
                }

                _teamService.RemoveEmployeeFromTeam(idEmployee);

                return RedirectToAction(nameof(ManageEmployeesForGeneralManager));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult EmployeeActive(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);

            if (user.Position.AccessLevel == Constants.OfficeManagerAccessLevel)
            {
                int idEmployee = id ?? default(int);
                _employeeService.ActivateEmployee(idEmployee);

                return RedirectToAction(nameof(ManageEmployeesForOfficeManager));
            }
            else
            {
                return NotFound();
            }
        }
    }
}