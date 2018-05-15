using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using Microsoft.AspNetCore.Mvc;

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
            if (true)
            {
                List<Team> teams = _employeeService.GetAllTeams().ToList();

                var model = new AllTeams
                {
                    Teams = teams
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
        public IActionResult TeamInfo(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idTeam = id ?? default(int);

                var team = _employeeService.GetTeam(idTeam);
                var model = new SingleTeam
                {
                    Team = team
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Teams/TeamAdd
        [HttpGet]
        public IActionResult TeamAdd()
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                var model = new AddTeam()
                {

                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }


        // POST: Teams/TeamDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}")]
        public IActionResult TeamDelete(int? id)
        {
            //TODO 
            //check accessLevel before 

            int idTeam = id ?? default(int);
            _employeeService.DeleteTeam(idTeam);
            return RedirectToAction(nameof(ManageTeams));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TeamAdd(AddTeam model)
        {
            //if (_auth.IsUserSignedIn(User))

            if (true)
            {
                // Add roles required !!! - delete this when you add 
                // the function to populate the model with roles,
                // in case not all inputs are added

                if (ModelState.IsValid)
                {
                    var team = new Team
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Employees = new List<Employee>()
                        //TODO
                        //TeamLeader is required
                        //Department is required
                    };
                    _employeeService.AddTeam(team);
                }

                else
                {
                    return View();
                }
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction(nameof(ManageTeams));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult TeamEdit(int? id)
        {
            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                int idTeam = id ?? default(int);

                var team = _employeeService.GetTeam(idTeam);

                if (team.Description == null)
                    team.Description = "";

                var model = new EditTeam
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description,

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
        public IActionResult TeamEdit(int? id, [Bind("Id, Name, Description")] EditTeam editTeam)
        {

            //if (_auth.IsUserSignedIn(User))
            if (true)
            {
                if (ModelState.IsValid)
                {
                    var team = new Team { Id = editTeam.Id, Name = editTeam.Name, Description = editTeam.Description };
                    _employeeService.UpdateTeam(team);

                }
                else
                {
                    return View(editTeam);
                }
                return RedirectToAction(nameof(ManageTeams));
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult AssignTeamLeader(int? id)
        {

            int idTeam = id ?? default(int);

            Team team = _employeeService.GetTeam(idTeam);


            //TODO: filter the students which already participate to activity
            var employees = _employeeService.GetAllUnassignedEmployees();

            var model = new AssignTeamLeader
            {
                Team = team,
                Employees = employees
            };

            return View(model);
        }
    }
}