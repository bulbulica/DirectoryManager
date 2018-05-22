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
        /**
         *  EmployeeService ?! trebuie modificat pe alt service !!!
         *  Gen : _departmentService
         *  private readonly IDepartmentService _departmentService;
         *  sa dai cu CTRL+R sa modifici in toate partile atunci cand 
         *  vei modifica numele de la employeeService
         * */
        private readonly IEmployeeService _employeeService;

        public DepartmentsController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentEdit(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            if(user.Position.AccessLevel<Constants.DepartmentManagerAccessLevel)
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
            // If we got this far, something failed, redisplay form
            return RedirectToAction(nameof(ManageDepartments));
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

            if (user.Id == departmentManager.Id)
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

        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentInfoAdvanced(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idDepartment = id ?? default(int);
            var department = _employeeService.GetDepartment(idDepartment);
            var departmentManager = _employeeService.GetDepartmentManager(department);

            if (user.Id == departmentManager.Id)
            {
                return RedirectToAction("DepartmentInfo", idDepartment);
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


        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DepartmentEdit(int? id, [Bind("Id, Name, Description")] EditDepartment newDepartment)
        {

            //if (_auth.IsUserSignedIn(User))
            if (true)
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
            //if (_auth.IsUserSignedIn(User))
            //{
            List<Department> departments = (List<Department>)_employeeService.GetAllDepartments();

            var model = new AllDepartments
            {
                Departments = departments
            };
            return View(model);

            //}
            //else
            //{
            //    return NotFound();
            //}
        }


        // GET: Departments/DepartmentAdd
        [HttpGet]
        public IActionResult DepartmentAdd()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                var model = new AddDepartment()
                {

                };

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
            //TODO 
            //check accessLevel before 

            int idDepartment = id ?? default(int);
            _employeeService.DeleteDepartment(idDepartment);
            return RedirectToAction(nameof(ManageDepartments));
        }
    }
}