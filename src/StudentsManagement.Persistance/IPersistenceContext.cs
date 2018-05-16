using IdentityServer.Domain;
using IdentityServer.Shared.Abstractions;

namespace IdentityServer.Persistence
{
    public interface IPersistenceContext : IInitializer
    {
        IEmployeeRepository EmployeeRepository { get; set; }
        int Complete();
        void Dispose();
    }
}
