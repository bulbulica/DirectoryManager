using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityServer.Persistence.EF
{
    public class EmployeeContextDbFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
    {
        EmployeeDbContext IDesignTimeDbContextFactory<EmployeeDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployeeDbContext>();
            optionsBuilder.UseSqlServer<EmployeeDbContext>("Server = (localdb)\\mssqllocaldb; Database = DirectoryEmployeesDb; Trusted_Connection = True; MultipleActiveResultSets = true");

            return new EmployeeDbContext(optionsBuilder.Options);
        }
    }
}