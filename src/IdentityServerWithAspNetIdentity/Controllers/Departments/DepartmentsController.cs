﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.Departments
{
    [Route("[controller]/[action]")]
    [SecurityHeaders]
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IAuthentication _auth;
        private readonly IBusinessLayer _businessLogic;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;
        private readonly ITeamService _teamService;

        public DepartmentsController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
            _departmentService = _businessLogic.GetDepartmentService();
            _teamService = _businessLogic.GetTeamService();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DepartmentAdd(AddDepartment model)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel)
            {
                if (ModelState.IsValid)
                {
                    var department = new Department
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Employees = new List<Employee>()

                    };
                    _departmentService.AddDepartment(department);
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction(nameof(ManageDepartments));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult AssignDepartmentManager(int? id)
        {
            int idDepartment = id ?? default(int);

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var department = _departmentService.GetDepartment(idDepartment);

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {

                    var employees = _employeeService.GetAllUnassignedEmployees().ToList();
                    employees.AddRange(department.Employees);
                    List<Employee> candidatesEmployees = new List<Employee>();

                    foreach (var employee in employees)
                    {
                        if (employee.Position.AccessLevel > Constants.DepartmentManagerAccessLevel
                        && employee.Active && employee.Position.AccessLevel != Constants.OfficeManagerAccessLevel) 
                            candidatesEmployees.Add(employee);
                    }

                    var model = new AssignDepartmentManager
                    {
                        Department = department,
                        Employees = candidatesEmployees
                    };
                    return View(model);
                
            }
            return NotFound();
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDepartmentManager(Department departmentModel, int IdDepartmentManager)
        {
            int departmentId = departmentModel.Id;
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var department = _departmentService.GetDepartment(departmentId);
            var departmentManager = _employeeService.GetEmployee(IdDepartmentManager);
            var exDepartmentManager = _departmentService.GetDepartmentManager(department);

            if (department == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (departmentManager != null)
                {
                    if (exDepartmentManager != null)
                    {
                        await _auth.UpdateRoleAsync(exDepartmentManager.Username, Constants.DeveloperRole);
                    }
                    _departmentService.UpdateDepartmentManager(department, departmentManager);
                    await _auth.UpdateRoleAsync(departmentManager.Username, Constants.DepartmentManagerRole);

                    return RedirectToAction(nameof(ManageDepartments));
                }
                return View();
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentAddEmployee(int? id)
        {
            int idDepartment = id ?? default(int);

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var department = _departmentService.GetDepartment(idDepartment);

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                var employees = _employeeService.GetAllEmployees().ToList();
                employees.AddRange(department.Employees);
                List<Employee> candidatesEmployees = new List<Employee>();

                foreach (var employee in employees)
                {
                    if (employee.Position.AccessLevel > Constants.DepartmentManagerAccessLevel
                    && employee.Active 
                    && employee.Department != department
                    && employee.Position.AccessLevel != Constants.OfficeManagerAccessLevel
                    && employee.Position.AccessLevel != Constants.GeneralManagerAccessLevel)
                        candidatesEmployees.Add(employee);
                }

                var model = new DepartmentAddEmployee
                {
                    Department = department,
                    Employees = candidatesEmployees
                };
                return View(model);

            }
            return NotFound();
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DepartmentAddEmployee(Department ModelDepartment, int EmployeeId)
        {
            int idDepartment = ModelDepartment.Id;
            var employee = _employeeService.GetEmployee(EmployeeId);
            var department = _departmentService.GetDepartment(idDepartment);

            if (department == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _departmentService.AddEmployeeToDepartment(employee, department);
                return RedirectToAction(nameof(ManageDepartments));
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentInfo(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idDepartment = id ?? default(int);
            var department = _departmentService.GetDepartment(idDepartment);
            var departmentManager = _departmentService.GetDepartmentManager(department);

            if (departmentManager != null)
            {
                if (user.Id == departmentManager.Id)
                {
                    return RedirectToAction("DepartmentInfo", idDepartment);
                }
            }
            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel)
            {
                var model = new SingleDepartment
                {
                    Department = department,
                    DepartmentManager = departmentManager
                };
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{username}")]
        public IActionResult GetDepartmentInfo(string username)
        {
            var user = _employeeService.GetEmployeeByName(username);
            if (user != null)
            {
                if(user.Position.AccessLevel==2)
                return RedirectToAction("DepartmentInfo", new { id = user.Department.Id });
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentEdit(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel)
            {
                int idDepartment = id ?? default(int);

                var department = _departmentService.GetDepartment(idDepartment);

                if (department.Description == null)
                    department.Description = "";

                var model = new EditDepartment
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,

                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult AddTeamToDepartment(int? id)
        {
            int idDepartment = id ?? default(int);

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var department = _departmentService.GetDepartment(idDepartment);

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {

                var teams = _teamService.GetAllTeams();
                List<Team> availableTeams = new List<Team>();

                foreach(var team in teams)
                {
                    if(team.Department != department)
                    {
                        availableTeams.Add(team);
                    }
                }

                var model = new AddTeamToDepartment
                {
                    Teams = availableTeams,
                    Department = department
                };
                return View(model);
            }
            else if(user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel
                    && user.Department.Id == department.Id)
            {
                var teams = _teamService.GetAllUnassignedTeams();
                var model = new AddTeamToDepartment
                {
                    Teams = teams,
                    Department = department
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult AddTeamToDepartment(Department departmentModel, int TeamId)
        {
            int departmentId = departmentModel.Id;
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var team = _teamService.GetTeam(TeamId);
            var department = _departmentService.GetDepartment(departmentId);
            var departmentManager = _departmentService.GetDepartmentManager(department);


            if (department == null || team == null)
            {
                return NotFound();
            }

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel
                || (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel
                    && user.Department.Id == department.Id)
                )
            {
                if (ModelState.IsValid)
                {
                    _departmentService.AddTeamToDepartment(department, team);
                    return RedirectToAction("DepartmentInfo", department.Id);
                }
                return View();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DepartmentEdit(int? id, [Bind("Id, Name, Description")] EditDepartment newDepartment)
        {

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                if (ModelState.IsValid)
                {
                    var department = new Department { Id = newDepartment.Id, Name = newDepartment.Name, Description = newDepartment.Description };
                    _departmentService.UpdateDepartment(department);
                }
                else
                {
                    return View(newDepartment);
                }
                return RedirectToAction(nameof(ManageDepartments));
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult ManageDepartments()
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var departments = _departmentService.GetAllDepartments();

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                var model = new AllDepartments
                {
                    Departments = departments
                };
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult DepartmentAdd()
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                var model = new AddDepartment();
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentDelete(int? id)
        {
            int idDepartment = id ?? default(int);
            var username = User.Identity.Name;
            var department = _departmentService.GetDepartment(idDepartment);
            var user = _employeeService.GetEmployeeByName(username);
            var departmentManager = _departmentService.GetDepartmentManager(department);
            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                if (departmentManager != null)
                {
                    _auth.UpdateRoleAsync(departmentManager.Username, Constants.DeveloperRole);
                }
                _departmentService.DeleteDepartment(idDepartment);
                return RedirectToAction(nameof(ManageDepartments));
            }
            else
            {
                return NotFound();
            }
        }
    }
}