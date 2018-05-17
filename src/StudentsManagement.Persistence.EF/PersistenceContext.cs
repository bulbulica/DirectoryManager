using IdentityServer.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Persistence.EF
{
    public class PersistenceContext : IPersistenceContext
    {

        private EmployeeDbContext _context;


        public IEmployeeRepository EmployeeRepository { get; set; }

        private void InitializeDbContext(IServiceProvider provider)
        {
            if (_context == null)
            {
                _context = provider.GetRequiredService<EmployeeDbContext>();
            }
            EmployeeRepository = new EmployeeRepository(_context);
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
            services.AddDbContext<EmployeeDbContext>(options =>
            options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("DirectoryEmployeesDb"),
                b => b.MigrationsAssembly("Identity.Persistence.EF")));

            InitializeDbContext(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder builder)
        {
            throw new NotImplementedException();
        }

        public void InitializeData(IServiceProvider serviceProvider)
        {
            InitializeDbContext(serviceProvider);

            var position1 = new Position
                {
                    RoleName = "Developer",
                    AccessLevel = 4,
                    Description = "Software dev"
                };

                var position2 = new Position
                {
                    RoleName = "TeamLead",
                    AccessLevel = 3,
                    Description = "Team Leader"
                };

                var position3 = new Position
                {
                    RoleName = "DepManager",
                    AccessLevel = 2,
                    Description = "Department Manager"
                };

                var position4 = new Position
                {
                    RoleName = "GeneralManager",
                    AccessLevel = 1,
                    Description = "General Manager"
                };

            EmployeeRepository.AddPositions(
                new List<Position>{
                    position4,
                    position3,
                    position2,
                    position1
                });

           

            if(EmployeeRepository.GetAllEmployees().Count() == 0)
            {
                var employee0 = new Employee
                {
                    Name = "Mirkea",
                    Picture = null,
                    Username = "as2qawd@asd.com",
                    Active = true,
                    CV = null,
                    Position = position1
                };

                var employee1 = new Employee
                {
                    Name = "Gheorghe",
                    Picture = null,
                    Username = "asd@asd.com",
                    Active = true,
                    CV = null,
                    Position = position1
                };

                var employee2 = new Employee
                {
                    Name = "Stefan",
                    Picture = null,
                    Username = "asaawd@asd.bcom",
                    Active = true,
                    CV = null,
                    Position = position1
                };

                var employee3 = new Employee
                {
                    Name = "Sandu",
                    Picture = null,
                    Username = "qwqwq@asd.bcom",
                    Active = true,
                    CV = null,
                    Position = position1
                };

                var employee4 = new Employee
                {
                    Name = "Binladen",
                    Picture = null,
                    Username = "soto@soto.ro",
                    Active = true,
                    CV = null,
                    Position = position1
                };

                EmployeeRepository.Add(employee0);
                EmployeeRepository.Add(employee1);
                EmployeeRepository.Add(employee2);
                EmployeeRepository.Add(employee3);
                EmployeeRepository.Add(employee4);

                var team1 = new Team
                {
                   Name = "Dusmanii",
                   Description = "Inamicii bugurilor",
                   Employees = new List<Employee>
                   {
                       employee1
                   }
                };


                var team2 = new Team
                {
                    Name = "Mustaciosii",
                    Description = "Mustata = viata",
                    Employees = new List<Employee>
                   {
                       employee0
                   }
                };

                EmployeeRepository.AddTeam(team1);
                EmployeeRepository.AddTeam(team2);

                employee1.Team = team1;
                employee0.Team = team2;

                var dep = new Department
                {
                    Name = "Java dev",
                    Description = "Java Masterrace only",
                    Teams = new List<Team>
                    {
                        team1,
                        team2
                    }
                };

            EmployeeRepository.AddDepartment(dep);

            employee0.Department = dep;
            employee1.Department = dep;
            employee2.Department = dep;
            employee3.Department = dep;
            employee4.Department = dep;

            Complete();
            }


        }

    }
}
