using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.Departments
{
    [Route("[controller]/[action]")]
    [SecurityHeaders]
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

        // GET: Departments/DepartmentInfo/{id}
        [HttpGet]
        [Route("{id}")]
        public IActionResult DepartmentInfo(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idDepartment = id ?? default(int);

                //var department = _employeeService.GetDepartment(idDepartment);
                var model = new SingleDepartment
                {
                    //Department = department
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

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            //var student = await _context.Student.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Student.Remove(student);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageDepartments));
        }
    }
}