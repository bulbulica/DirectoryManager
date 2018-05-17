using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Core.Shared
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployee(int idEmployee);
        IEnumerable<Position> GetAllPositions();
        void AddEmployee(Employee employee);
        Position GetPositionByName(string position);
        Department GetDepartmentByName(string department);
        IEnumerable<Department> GetAllDepartments();
        void UpdateEmployee(Employee employee);
        IEnumerable<Team> GetAllTeams();
        void DeleteEmployee(int idEmployee);
        Department GetDepartment(int idDepartment);
        Team GetTeam(int idTeam);
        void UpdateDepartment(Department department);
        void UpdateTeam(Team team);
        void DeleteDepartment(int idDepartment);
        void DeleteTeam(int idTeam);
        void AddDepartment(Department department);
        void AddTeam(Team team);
        IEnumerable<Employee> GetAllUnassignedEmployees();
        void UpdateTeamLeader(Team team, Employee employee);
        Employee GetTeamLeader(Team team);
        Employee GetDepartmentManager(Department department);
    }
}
