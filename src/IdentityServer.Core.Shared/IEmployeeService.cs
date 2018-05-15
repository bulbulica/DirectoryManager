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
    }
}
