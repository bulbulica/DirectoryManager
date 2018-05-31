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
        Position GetDeveloperPosition();
        List<Employee> GetAllEmployees();
        Employee GetEmployee(int idEmployee);
        IEnumerable<Position> GetAllPositions();
        void AddEmployee(Employee employee);
        Position GetPositionByName(string position);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int idEmployee);
        void ActivateEmployee(int idEmployee);
        IEnumerable<Employee> GetAllUnassignedEmployees();
        IEnumerable<Position> GetRegisterPositionsByAccessLevel(string username);
        Employee GetEmployeeByName(string employeeName);
        void UpdateCV(Employee employee, string filePath);
        void UpdateImage(Employee employee, string filePath);
        void UpdateEmployeeName(Employee employee, string name);
        void UpdateEmployeePosition(Position position, Employee employee);
        IEnumerable<Employee> GetAllEmployeesWithLowerAccessLevel(Employee employee);
        IEnumerable<Employee> GetAllEmployeesWithSameAccessLevel(Employee employee);
        IEnumerable<Position> GetAllRegisterPositions();
    }
}
