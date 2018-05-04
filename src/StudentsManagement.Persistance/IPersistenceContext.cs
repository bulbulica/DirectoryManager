using StudentsManagement.Shared.Abstractions;

namespace StudentsManagement.Persistence
{
    public interface IPersistenceContext : IInitializer
    {
        int Complete();
        void Dispose();
        IActivityRepository ActivityRepository { get; set; }
        ITeachersRepository TeachersRepository { get; set; }
        IStudentsRepository StudentsRepository { get; set; }
    }
}
