using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer
{
    [Route("[controller]/[action]")]
    [SecurityHeaders]
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly IAuthentication _auth;
        private readonly IBusinessLayer _businessLogic;
        private readonly IEmployeeService _employeeService;
        private readonly ITeamService _teamService;
        private readonly IDepartmentService _departmentService;

        public TeamsController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
            _departmentService = _businessLogic.GetDepartmentService();
            _teamService = _businessLogic.GetTeamService();
        }

        [HttpGet]
        public IActionResult ManageTeams()
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);
            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            {
                List<Team> teams = _teamService.GetAllTeams().ToList();

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

        [HttpGet("{username}")]
        public IActionResult ManageTeamsForDepartmentManager(string username)
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);
            if (user == null || user.Position.AccessLevel != Constants.DepartmentManagerAccessLevel)
            {
                return NotFound();
            }
            List<Team> teams = _departmentService.GetAllTeamsFromDepartment(user.Department);

            var model = new AllTeams
            {
                Teams = teams
            };
            return View(model);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult TeamInfo(int? id)
        {

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idTeam = id ?? default(int);

            var team = _teamService.GetTeam(idTeam);
            var teamLeader = _teamService.GetTeamLeader(team);

            if (user.Position.AccessLevel > Constants.TeamLeaderAccessLevel)
            {
                return NotFound();
            }
            if ((user.Position.AccessLevel == Constants.TeamLeaderAccessLevel
                && user.Team.Id == _teamService.GetTeam(idTeam).Id))
            {

                var model = new SingleTeam
                {
                    Team = team,
                    TeamLeader = teamLeader
                };

                return View(model);
            }
            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel
                 || user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel && user.Department.Teams.Contains(team))
            {
                return RedirectToAction(nameof(TeamInfoAdvanced));
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult TeamInfoAdvanced(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idTeam = id ?? default(int);

            var team = _teamService.GetTeam(idTeam);
            var teamLeader = _teamService.GetTeamLeader(team);

            if (user.Position.AccessLevel == Constants.TeamLeaderAccessLevel)
            {
                return RedirectToAction(nameof(TeamInfo));
            }
            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel
                || user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel && user.Department.Teams.Contains(team))
            {

                var model = new SingleTeam
                {
                    Team = team,
                    TeamLeader = teamLeader
                };

                return View(model);
            }
            return NotFound();
        }

        [HttpGet("{username}")]
        public IActionResult GetTeamInfo(string username)
        {
            var user = _employeeService.GetEmployeeByName(username);
            if (user != null)
            {
                if (user.Position.AccessLevel == Constants.TeamLeaderAccessLevel)
                {
                    return RedirectToAction("TeamInfo", new { id = user.Team.Id });
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult TeamAdd()
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);

            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel)
            {
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
                {
                    var model = new AddTeam()
                    {
                        Departments =
                        new List<Department>{
                            user.Department
                        }
                    };
                    return View(model);
                }
                else
                {
                    var model = new AddTeam()
                    {
                        Departments = _departmentService.GetAllDepartments().ToList()
                    };
                    return View(model);
                }

            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TeamAdd(AddTeam model)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);

            var department = _departmentService.GetDepartment(model.DepartmentId);
            if (model.DepartmentId == -1)
            {
                department = null;
            }

            if (ModelState.IsValid)
            {
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel &&
                user.Department.Id == department.Id)
                {
                    var team = new Team
                    {
                        Name = model.Name,
                        Department = department,
                        Description = model.Description,
                        Employees = new List<Employee>()
                    };
                    _teamService.AddTeam(team);
                    return RedirectToAction(nameof(ManageTeamsForDepartmentManager), new { username });
                }


                if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
                {
                    var team = new Team
                    {
                        Name = model.Name,
                        Department = department,
                        Description = model.Description,
                        Employees = new List<Employee>()
                    };
                    _teamService.AddTeam(team);
                    return RedirectToAction(nameof(ManageTeams));
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult TeamEdit(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idTeam = id ?? default(int);
            var team = _teamService.GetTeam(idTeam);

            if (user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel
                || user.Department.Id == team.Department.Id)
            {
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

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var oldTeam = _teamService.GetTeam(editTeam.Id);
            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel
                || user.Department.Id == oldTeam.Department.Id)
            {
                if (ModelState.IsValid)
                {
                    var team = new Team { Id = editTeam.Id, Name = editTeam.Name, Description = editTeam.Description };
                    _teamService.UpdateTeam(team);
                }
                else
                {
                    return View(editTeam);
                }
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
                {
                    return RedirectToAction(nameof(ManageTeamsForDepartmentManager), new { username });
                }
                else
                {
                    return RedirectToAction(nameof(ManageTeams));
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult AssignTeamLeader(int? id)
        {
            int idTeam = id ?? default(int);

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var team = _teamService.GetTeam(idTeam);

            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel)
            {
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel && user.Department.Id == team.Department.Id
                    || user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel)
                {
                    var employees = _employeeService.GetAllUnassignedEmployees().ToList();
                    employees.AddRange(team.Employees);
                    List<Employee> candidatesEmployees = new List<Employee>();

                    foreach (var employee in employees)
                    {
                        if (employee.Position.AccessLevel > Constants.DepartmentManagerAccessLevel
                            && employee.Active && employee.Position.AccessLevel != Constants.OfficeManagerAccessLevel)
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

            return NotFound();
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTeamLeaderToTeam(Team ModelTeam, int IdTeamLeader)
        {
            int idTeam = ModelTeam.Id;
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var team = _teamService.GetTeam(idTeam);
            var TeamLeader = _employeeService.GetEmployee(IdTeamLeader);
            var ExTeamLeader = _teamService.GetTeamLeader(team);

            if (team == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (TeamLeader != null)
                {
                    if (ExTeamLeader != null)
                    {
                        await _auth.UpdateRoleAsync(ExTeamLeader.Username, Constants.DeveloperRole);
                    }
                    _teamService.UpdateTeamLeader(team, TeamLeader);
                    await _auth.UpdateRoleAsync(TeamLeader.Username, Constants.TeamLeaderRole);

                    if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
                    {
                        return RedirectToAction(nameof(ManageTeamsForDepartmentManager), new { username });
                    }
                    else
                    {
                        return RedirectToAction(nameof(ManageTeams));
                    }
                }
                return View();
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult TeamAddEmployee(int? id)
        {
            int idTeam = id ?? default(int);

            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            var team = _teamService.GetTeam(idTeam);

            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel)
            {
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel
                    && user.Department.Id == team.Department.Id
                    || user.Position.AccessLevel < Constants.DepartmentManagerAccessLevel)
                {
                    List<Employee> employees = new List<Employee>();
                    if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
                    {
                        employees = _employeeService.GetAllUnassignedEmployees().ToList();
                    }
                    else
                    {
                        employees = _employeeService.GetAllEmployees().ToList();
                    }

                    List<Employee> availableEmployees = new List<Employee>();

                    foreach (var employee in employees)
                    {
                        if (employee.Position.AccessLevel > Constants.DepartmentManagerAccessLevel
                            && employee.Active
                            && employee.Team != team
                            && employee.Position.AccessLevel != Constants.OfficeManagerAccessLevel)
                            availableEmployees.Add(employee);
                    }

                    var model = new AddEmployeeToTeam
                    {
                        Team = team,
                        Employees = availableEmployees
                    };
                    return View(model);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult TeamAddEmployee(Team ModelTeam, int EmployeeId)
        {
            int idTeam = ModelTeam.Id;
            var employee = _employeeService.GetEmployee(EmployeeId);
            var team = _teamService.GetTeam(idTeam);

            if (team == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _teamService.AddEmployeeToTeam(employee, team);
                return RedirectToAction("TeamInfoAdvanced", idTeam);
            }
            return NotFound();
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult TeamDelete(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idTeam = id ?? default(int);
            var team = _teamService.GetTeam(idTeam);
            var teamLeader = _teamService.GetTeamLeader(team);

            if (user.Position.AccessLevel < Constants.TeamLeaderAccessLevel)
            {
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel && user.Department != team.Department)
                {
                    return NotFound();
                }
                if (teamLeader != null)
                {
                    _auth.UpdateRoleAsync(teamLeader.Username, Constants.DeveloperRole);
                }
                _teamService.DeleteTeam(idTeam);
                if (user.Position.AccessLevel == Constants.DepartmentManagerAccessLevel)
                {
                    return RedirectToAction(nameof(ManageTeamsForDepartmentManager), new { username });
                }
                else
                {
                    return RedirectToAction(nameof(ManageTeams));
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult RemoveTeamFromDepartment(int? id)
        {
            var username = User.Identity.Name;
            var user = _employeeService.GetEmployeeByName(username);
            int idTeam = id ?? default(int);
            var team = _teamService.GetTeam(idTeam);

            if (user.Position.AccessLevel == Constants.GeneralManagerAccessLevel)
            { 
                _departmentService.RemoveTeamFromDepartment(idTeam);
                return RedirectToAction("ManageTeams");
            }
            return NotFound();
        }
    }
}