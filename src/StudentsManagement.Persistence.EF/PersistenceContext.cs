using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentsManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsManagement.Persistence.EF
{
    public class PersistenceContext : IPersistenceContext
    {

        private StudentsManagementDbContext _context;


        public IActivityRepository ActivityRepository { get; set; }
        public ITeachersRepository TeachersRepository { get; set; }
        public IStudentsRepository StudentsRepository { get; set; }

        private void InitializeDbContext(IServiceProvider provider)
        {
            if (_context == null)
            {
                _context = provider.GetRequiredService<StudentsManagementDbContext>();
            }
            ActivityRepository = new ActivityRepository(_context);
            TeachersRepository = new TeachersRepository(_context);
            StudentsRepository = new StudentsRepository(_context);
        }

        public PersistenceContext() { }


        public int Complete()
        {
            int retVal = 0;
            if (_context != null)
            {
                retVal = _context.SaveChanges();
            }

            return retVal;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void InitializeContext(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<StudentsManagementDbContext>(options =>
            options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("StudentsManagementConnection"),
                b => b.MigrationsAssembly("StudentsManagement.Persistence.EF")));

            InitializeDbContext(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder builder)
        {
            throw new NotImplementedException();
        }

        public void InitializeData(IServiceProvider serviceProvider)
        {
            InitializeDbContext(serviceProvider);

            ActivityType course = new ActivityType { Name = "Course" };
            ActivityType laboratory = new ActivityType { Name = "Laboratory" };
            ActivityType seminary = new ActivityType { Name = "Seminary" };

            var stud1 = StudentsRepository.GetStudentByName("Gheorghe");
            var stud2 = StudentsRepository.GetStudentByName("Stefanescu");
            var stud3 = StudentsRepository.GetStudentByName("Florescu");

            var teacher1 = TeachersRepository.GetTeacherByName("Costache");
            var teacher2 = TeachersRepository.GetTeacherByName("Ofelia");
            var teacher3 = TeachersRepository.GetTeacherByName("Mihaescu");

            var activity1 = ActivityRepository.GetActivityByName("Sisteme de Operare");
            var activity2 = ActivityRepository.GetActivityByName("Arhitectura calc");
            var activity3 = ActivityRepository.GetActivityByName("Arhitectura Calculatoarelor II");

            if (StudentsRepository.ListAll().Count() == 0)
            {
                stud1 = new Student
                {
                    Name = "Gheorghe"
                };

                stud2 = new Student
                {
                    Name = "Stefanescu"
                };

                stud3 = new Student
                {
                    Name = "Florescu"
                };

                StudentsRepository.Add(stud1);
                StudentsRepository.Add(stud2);
                StudentsRepository.Add(stud3);
            }

            if (teacher1 == null)
            {
                teacher1 = new Teacher
                {
                    Name = "Costache"
                };
                TeachersRepository.Add(teacher1);
            }
            
            if (teacher2 == null)
            {
                teacher2 = new Teacher
                {
                    Name = "Ofelia"
                };
                TeachersRepository.Add(teacher2);
            }

            if (teacher3 == null)
            {
                teacher3 = new Teacher
                {
                    Name = "Mihaescu"
                };
                TeachersRepository.Add(teacher3);
            }

            if (ActivityRepository.ListAll().Count() == 0)
            {
                activity1 = new Activity
                {
                    Name = "Sisteme de Operare",
                    Description = "Studiu al SO",
                    Owner = teacher1,
                    ActivityType = course

                };

                activity2 = new Activity
                {
                    Name = "Arhitectura Calculatoarelor",
                    Description = "Arhitectura calc",
                    ActivityType = laboratory,
                    Owner = teacher2
                };

                activity3 = new Activity
                {
                    Name = "Arhitectura Calculatoarelor II",
                    Description = "Arhitectura II",
                    ActivityType = seminary,
                    Owner = teacher3
                };

                ActivityRepository.Add(activity1);
                ActivityRepository.Add(activity2);
                ActivityRepository.Add(activity3);
            }

            if (_context.StudentActivityDetails.ToList().Count == 0)
            {
                var StudentActivityDetail1 = new StudentActivityDetails
                {
                    Activity = activity2,
                    Student = stud1,
                };

                var StudentActivityDetail2 = new StudentActivityDetails
                {
                    Activity = activity2,
                    Student = stud2,
                };

                var StudentActivityDetail3 = new StudentActivityDetails
                {
                    Activity = activity1,
                    Student = stud2,
                };

                var StudentActivityDetail4 = new StudentActivityDetails
                {
                    Activity = activity1,
                    Student = stud3,
                };

                var StudentActivityDetail5 = new StudentActivityDetails
                {
                    Activity = activity3,
                    Student = stud3,
                };

                Complete();
            }
        }
    }
}
