using System;
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
        private readonly IEmployeeService _employeeService;

        public DepartmentsController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
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
                    _employeeService.AddDepartment(department);
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
            /*
             * AICI TREBUIE SA ASIGNAM DEPARTMENT MANAGER DE CATRE GENERAL MANAGER
             * modifici si tu codul cat sa mearga, incearca doar cei din interiorul 
             * departamentului sa poata sa devina department manageri, ca daca sunt
             * externi apar din add employee ca department manager
             * /
            int idTeam = id ?? default(int);

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var team = _employeeService.GetTeam(idTeam);

            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel)
            {
                // If User = Deparment Manager
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel && user.Department == team.Department
                    || user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel)
                {
                    var employees = _employeeService.GetAllUnassignedEmployees().ToList();
                    employees.AddRange(team.Employees);
                    List<Employee> candidatesEmployees = new List<Employee>();

                    foreach (var employee in employees)
                    {
                        if (employee.Position.AccessLevel > Constants.DepartmentManagerAccessLevel)
                            candidatesEmployees.Add(employee);
                    }

                    var model = new AssignTeamLeader
                    {
                        Team = team,
                        Employees = candidatesEmployees
                    };
                    return View(model);
                }
            }
            */
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentInfo(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idDepartment = id ?? default(int);
            var department = _employeeService.GetDepartment(idDepartment);
            var departmentManager = _employeeService.GetDepartmentManager(department);

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

                var department = _employeeService.GetDepartment(idDepartment);

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
                    _employeeService.UpdateDepartment(department);
                }
                else
                {
                    return View(newDepartment);
                }
                return RedirectToAction(nameof(ManageDepartments));
            }
            return NotFound();
        }

        // GET: Departments/ManageDepartments
        [HttpGet]
        public IActionResult ManageDepartments()
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var departments = _employeeService.GetAllDepartments();

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


        // GET: Departments/DepartmentAdd
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

        // GET: Departments/DepartmentDellete/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentDelete(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                int idDepartment = id ?? default(int);
                _employeeService.DeleteDepartment(idDepartment);
                return RedirectToAction(nameof(ManageDepartments));
            }
            else
            {
                return NotFound();
            }
        }
    }
}