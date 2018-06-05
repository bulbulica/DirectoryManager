using IdentityServer.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                b => b.MigrationsAssembly("IdentityServer.Persistence.EF")));

            InitializeDbContext(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder builder)
        {
            throw new NotImplementedException();
        }

        public void InitializeData(IServiceProvider serviceProvider)
        {
            InitializeDbContext(serviceProvider);
            _context?.Database.Migrate();
            if (_context.Positions.Count() == 0)
            {
                var qaPosition = new Position
                {
                    RoleName = "QA",
                    AccessLevel = 4,
                    Description = "Quality assurance"
                };

                var devPosition = new Position
                {
                    RoleName = "Developer",
                    AccessLevel = 4,
                    Description = "Software dev"
                };

                var teamLeaderPosition = new Position
                {
                    RoleName = "TeamLeader",
                    AccessLevel = 3,
                    Description = "Team Leader"
                };

                var departmentManagerPosition = new Position
                {
                    RoleName = "DepartmentManager",
                    AccessLevel = 2,
                    Description = "Department Manager"
                };

                var generalManagerPosition = new Position
                {
                    RoleName = "GeneralManager",
                    AccessLevel = 1,
                    Description = "General Manager"
                };

                var officeManagerPosition = new Position
                {
                    RoleName = "OfficeManager",
                    AccessLevel = 255,
                    Description = "Office Manager"
                };

                EmployeeRepository.AddPositions(
                new List<Position>{
                officeManagerPosition,
                generalManagerPosition,
                departmentManagerPosition,
                teamLeaderPosition,
                devPosition,
                qaPosition
                });

                Complete();
            }
        }
    }
}
