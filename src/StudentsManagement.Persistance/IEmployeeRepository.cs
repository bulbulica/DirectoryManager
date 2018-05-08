using IdentityServer.Domain;


namespace IdentityServer.Persistence
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetEmployeeByName(string name);

    }
}
