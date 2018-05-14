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
        private readonly IEmployeeService _employeeService;

        public DepartmentsController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
        }


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
            return View("ManageDepartments", model);

            //}
            //else
            //{
            //    return NotFound();
            //}
        }
    }
}