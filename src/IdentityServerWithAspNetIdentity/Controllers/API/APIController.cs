using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.API
{

    [Produces("application/json")]
    [Authorize]
    public class APIController : Controller
    {
        private readonly IBusinessLayer _businessLayer;
        private readonly IEmployeeService _employeeService;

        public APIController(IBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
            _employeeService = _businessLayer.GetEmployeeService();
        }

        [HttpGet]
        public Employee GetEmployeeInfo (string username)
        {
            var employee = _employeeService.GetEmployeeByName(username);
            if(employee == null)
            {
                return null;
            }
            return employee;
        }

        [HttpGet]
        public ICollection<Employee> GetEmployeesToEvaluate(string username)
        {
            var employee = _employeeService.GetEmployeeByName(username);
            if (employee == null)
            {
                return null;
            }
            List<Employee> employees = _employeeService.GetAllEmployeesWithLowerAccessLevel(employee).ToList();

            return employees;
        }
    }
}