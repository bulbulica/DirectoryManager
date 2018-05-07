using IdentityServer.Domain;


namespace IdentityServer.Persistence
{
    public interface IUserRepository : IRepository<Employee>
    {
        Employee GetTeacherByName(string name);

    }
}
