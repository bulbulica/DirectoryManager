using StudentsManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsManagement.Persistence
{
    public interface IStudentsRepository : IRepository<Student>
    {
        IEnumerable<Student> ListAllFromActivity(int id);
        Student GetStudentByName(string name);

    }
}
