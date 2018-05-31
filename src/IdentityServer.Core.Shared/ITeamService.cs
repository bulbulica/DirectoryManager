using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Core.Shared
{
    public interface ITeamService
    {
        IEnumerable<Team> GetAllTeams();
        Team GetTeam(int idTeam);
        void UpdateTeam(Team team);
        void DeleteTeam(int idTeam);
        void AddTeam(Team team);
        void UpdateTeamLeader(Team team, Employee employee);
        Employee GetTeamLeader(Team team);
        Position GetTeamLeaderPosition();
        List<Employee> GetAllEmployeesFromTeam(Team team);
        IEnumerable<Team> GetAllUnassignedTeams();
        void AddEmployeeToTeam(Employee employee, Team team);
        void RemoveEmployeeFromTeam(int idEmployee);
    }
}
