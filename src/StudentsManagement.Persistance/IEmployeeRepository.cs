using IdentityServer.Domain;
using System.Collections.Generic;

namespace IdentityServer.Persistence
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetEmployeeByName(string nameEmployee);
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(int idEmployee);

    }
}
