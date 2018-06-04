using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityServer.Core
{
    public class TeamService : ITeamService
    {
        private IPersistenceContext _persistenceContext;

        public TeamService(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
        }

        public IPersistenceContext PersistenceContext { get => _persistenceContext; set => _persistenceContext = value; }

        public void AddEmployeeToTeam(Employee employee, Team team)
        {
            employee.Team = team;
            team.Employees.Add(employee);
            if (team.Department != null)
            {
                employee.Department = team.Department;
                team.Department.Employees.Add(employee);
            }
            employee.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
            PersistenceContext.Complete();
        }

        public void AddTeam(Team team)
        {
            PersistenceContext.EmployeeRepository.AddTeam(team);
            PersistenceContext.Complete();
        }

        public void DeleteTeam(int idTeam)
        {
            var team = PersistenceContext.EmployeeRepository.GetTeamById(idTeam);
            if (team != null)
            {
                var teamLeader = GetTeamLeader(team);

                if (teamLeader != null)
                {
                    teamLeader.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                foreach (var employee in team.Employees)
                {
                    employee.Team = null;
                }

                PersistenceContext.EmployeeRepository.DeleteTeam(team);
                PersistenceContext.Complete();
            }
        }

        public List<Employee> GetAllEmployeesFromTeam(Team team)
        {
            return PersistenceContext.EmployeeRepository.GetAllEmployeesFromTeam(team);
        }

        public IEnumerable<Team> GetAllTeams()
        {
            if (PersistenceContext.EmployeeRepository.GetAllTeams().Count() != 0)
                return PersistenceContext.EmployeeRepository.GetAllTeams().ToList();
            return new List<Team>();
        }

        public IEnumerable<Team> GetAllUnassignedTeams()
        {
            return PersistenceContext.EmployeeRepository.GetAllUnassignedTeams();
        }

        public Team GetTeam(int idTeam)
        {
            return PersistenceContext.EmployeeRepository.GetTeamById(idTeam);
        }

        public Employee GetTeamLeader(Team team)
        {
            var teamLeadPosition = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();

            if (team == null)
            {
                return null;
            }

            foreach (var employee in team.Employees)
            {
                if (employee.Position == teamLeadPosition)
                    return employee;
            }
            return null;
        }

        public Position GetTeamLeaderPosition()
        {
            return PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
        }

        public void RemoveEmployeeFromTeam(int idEmployee)
        {
            var user = PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee);
            user.Team = null;
            PersistenceContext.Complete();
        }

        public void UpdateTeam(Team team)
        {
            var newTeam = PersistenceContext.EmployeeRepository.GetTeamById(team.Id);
            newTeam.Name = team.Name;
            newTeam.Description = team.Description;
            PersistenceContext.Complete();
        }

        public void UpdateTeamLeader(Team team, Employee employee)
        {
            var exTeamLeader = GetTeamLeader(team);

            if (team.Employees.Contains(employee))
            {
                if (exTeamLeader != null)
                {
                    exTeamLeader.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                employee.Position = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
            }
            else
            {
                if (exTeamLeader != null)
                {
                    exTeamLeader.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                team.Employees.Add(employee);
                employee.Team = team;
                employee.Department = team.Department;
                employee.Position = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
            }
            PersistenceContext.Complete();
        }
    }
}
