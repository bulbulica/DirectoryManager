using Microsoft.EntityFrameworkCore;
using StudentsManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsManagement.Persistence.EF
{
    class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(DbContext context) : base(context)
        {
            
        }


        public StudentsManagementDbContext StudentsManagementDbContext
        {
            get
            {
                return Context as StudentsManagementDbContext;
            }
        }

        public Student GetStudentByName(string name)
        {
            return StudentsManagementDbContext.Students
                .Where(b => b.Name == name)
                .SingleOrDefault();
        }

        public IEnumerable<Student> ListAllFromActivity(int id)
        {
            List<StudentActivityDetails> studentActivityDetails;
            studentActivityDetails = StudentsManagementDbContext.StudentActivityDetails.ToList();
            List<Student> studentsToBeReturned = new List<Student>();

            foreach(var activityDetail in studentActivityDetails)
            {
                if (activityDetail.ActivityId == id)
                    studentsToBeReturned.Add(activityDetail.Student);
            }

            return studentsToBeReturned;
        }
    }
}
