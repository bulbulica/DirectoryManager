using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        IEnumerable<Position> GetRegisterPositionsByAccessLevel(string username);
        Position GetDepartmentManagerPosition();
        void UpdateDepartmentManager(Department department, Employee employee);
        Position GetTeamLeaderPosition();
        Employee GetEmployeeByName(string employeeName);
        void UpdateCV(Employee employee, string filePath);
        void UpdateImage(Employee employee, string filePath);
        List<Employee> GetAllEmployeesFromTeam(Team team);
        List<Employee> GetAllEmployeesFromDepartment(Department department);

    }
}
