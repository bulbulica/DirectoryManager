using IdentityServer.Domain;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Persistence.EF
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Department>()
                .HasMany(d => d.Employees);

            builder.Entity<Team>()
               .HasMany(d => d.Employees);
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
    }
}
