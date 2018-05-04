using StudentsManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsManagement.Persistence
{
    public interface ITeachersRepository : IRepository<Teacher>
    {
        Teacher GetTeacherByName(string name);

    }
}
