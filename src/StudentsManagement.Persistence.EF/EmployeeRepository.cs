using IdentityServer.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Persistence.EF
{
    class UserRepository : Repository<Employee>, IEmployeeRepository
    {
        //public UserRepository(DbContext context) : base(context)
        //{
        //}

        //public void Add(Employee entity)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void Delete(Employee entity)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public UserEntity GetTeacherByName(string name)
        //{
        //    throw new System.NotImplementedException();
        //}

        //Employee IRepository<Employee>.GetEntity(int id)
        //{
        //    throw new System.NotImplementedException();
        //}

        //Employee IUserRepository.GetTeacherByName(string name)
        //{
        //    throw new System.NotImplementedException();
        //}

        //IEnumerable<Employee> IRepository<Employee>.ListAll()
        //{
        //    throw new System.NotImplementedException();
        //}


        //public StudentsManagementDbContext StudentsManagementDbContext
        //{
        //    get
        //    {
        //        return Context as StudentsManagementDbContext;
        //    }
        //}

        //public Teacher GetTeacherByName(string name)
        //{
        //    Teacher retVal = null;

        //    if (StudentsManagementDbContext != null)
        //    {
        //        retVal = StudentsManagementDbContext.Teachers.SingleOrDefault(t => t.Name.Equals(name));
        //    }

        //    return retVal;
        //}
        public Employee GetEmployeeByName(string name)
        {
            Employee retval = null;
            if()
        }
    }
}
