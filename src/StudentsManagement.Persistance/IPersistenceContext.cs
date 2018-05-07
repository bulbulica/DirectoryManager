using IdentityServer.Shared.Abstractions;

namespace IdentityServer.Persistence
{
    public interface IPersistenceContext : IInitializer
    {
        int Complete();
        void Dispose();
        IUserRepository UserRepository { get; set; }
    }
}
