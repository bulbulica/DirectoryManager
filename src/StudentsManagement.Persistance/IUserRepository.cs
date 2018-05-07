using IdentityServer.Domain;


namespace IdentityServer.Persistence
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        UserEntity GetTeacherByName(string name);

    }
}
