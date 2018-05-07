using IdentityServer.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IdentityServer.Persistence.EF
{
    class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public UserEntity GetTeacherByName(string name)
        {
            throw new System.NotImplementedException();
        }


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
    }
}
