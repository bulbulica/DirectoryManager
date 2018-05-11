using IdentityServer.Domain;
using System.Collections.Generic;

namespace IdentityServer.Persistence
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        int GetAllPositions { get; }

        Employee GetEmployeeByName(string nameEmployee);
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(int idEmployee);
        IEnumerable<Team> GetAllTeams();
        void AddPositions(List<Position> list);
        void AddTeam(Team team2);
        void AddDepartment(Department dep);
    }
}
