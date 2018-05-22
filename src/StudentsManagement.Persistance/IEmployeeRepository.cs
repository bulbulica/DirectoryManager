using IdentityServer.Domain;
using System.Collections.Generic;

namespace IdentityServer.Persistence
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
       
        Employee GetEmployeeByName(string nameEmployee);
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(int idEmployee);
        IEnumerable<Team> GetAllTeams();
        void AddPositions(List<Position> list);
        void AddTeam(Team team2);
        void AddDepartment(Department dep);
        IEnumerable<Position> GetAllPositions();
        Position GetPositionByName(string name);
        Department GetDepartmentByName(string department);
        IEnumerable<Department> GetAllDepartments();
        Department GetDepartmentById(int idDepartment);
        Team GetTeamById(int idTeam);
        void DeleteDepartment(Department department);
        void DeleteTeam(Team team);
        IEnumerable<Employee> GetAllUnassignedEmployees();
        Position GetDeveloperPosition();
        Position GetTeamLeaderPosition();
        Position GetDepartmentManagerPosition();
        List<Employee> GetAllEmployeesFromDepartment(Department department);
        List<Employee> GetAllEmployeesFromTeam(Team team);
        List<Team> GetAllTeamsFromDepartment(Department department);
    }
}
