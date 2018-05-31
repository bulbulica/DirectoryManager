using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Core.Shared
{
    public interface IDepartmentService
    {
        Department GetDepartmentByName(string department);
        IEnumerable<Department> GetAllDepartments();
        Department GetDepartment(int idDepartment);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int idDepartment);
        void AddDepartment(Department department);
        Employee GetDepartmentManager(Department department);
        Position GetDepartmentManagerPosition();
        void UpdateDepartmentManager(Department department, Employee employee);
        List<Team> GetAllTeamsFromDepartment(Department department);
        List<Employee> GetAllEmployeesFromDepartment(Department department);
        void AddTeamToDepartment(Department department, Team team);
        void AddEmployeeToDepartment(Employee employee, Department department);
        void RemoveTeamFromDepartment(int idTeam);
        void RemoveEmployeeFromDepartment(int idEmployee);
    }
}
