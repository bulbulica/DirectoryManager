using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Views.Teams
{
    namespace IdentityServer
    {
        [Route("[controller]/[action]")]
        [SecurityHeaders]
        public class TeamsController : Controller
        {
            private readonly IAuthentication _auth;
            private readonly IBusinessLayer _businessLogic;
            private readonly IEmployeeService _employeeService;

            public TeamsController(IAuthentication auth,
                IBusinessLayer businessLayer)
            {
                _auth = auth;
                _businessLogic = businessLayer;
                _employeeService = _businessLogic.GetEmployeeService();
            }

            // GET: Teams/ManageTeams
            [HttpGet]
            public IActionResult ManageTeams()
            {
                //if (_auth.IsUserSignedIn(User))
                //{
                List<Team> teams = _employeeService.GetAllTeams().ToList();

                var model = new AllTeams
                {
                    Teams = teams
                };
                return View("ManageTeams", model);

                //}
                //else
                //{
                //    return NotFound();
                //}
            }
        }
    }
}