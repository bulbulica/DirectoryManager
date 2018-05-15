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

        // POST: Teams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            //var student = await _context.Student.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Student.Remove(student);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageTeams));
        }
    }
}