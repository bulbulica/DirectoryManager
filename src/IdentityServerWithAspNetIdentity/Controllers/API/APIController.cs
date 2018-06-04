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
    [Route("/[controller]")]
    public class APIController: Controller
    {
        private readonly IBusinessLayer _businessLayer;
        private readonly IEmployeeService _employeeService;

        public APIController(IBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
            _employeeService = _businessLayer.GetEmployeeService();
        }

        [HttpGet("GetEmployeeInfo/{username}")]
        public IActionResult GetEmployeeInfo (string username)
        {
            var employee = _employeeService.GetEmployeeByName(username);
            if(employee == null)
            {
                return null;
            }
            return Ok(employee);
        }

        [HttpGet("GetEmployeesToEvaluate/{username}")]
        public IActionResult GetEmployeesToEvaluate(string username)
        {
            var employee = _employeeService.GetEmployeeByName(username);
            if (employee == null)
            {
                return null;
            }
            List<Employee> employees = _employeeService.GetAllEmployeesWithLowerAccessLevel(employee).ToList();

            return Ok(employees);
        }

        [HttpGet("GetAllEqualRankEmployees/{username}")]
        public IActionResult GetAllEqualRankEmployees(string username)
        {
            var employee = _employeeService.GetEmployeeByName(username);
            if (employee == null)
            {
                return null;
            }
            List<Employee> employees = _employeeService.GetAllEmployeesWithSameAccessLevel(employee).ToList();

            return Ok(employees);
        }
    }
}