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


            builder.Entity<Employee>()
                .HasOne(e => e.Team);

            builder.Entity<Employee>()
                .HasOne(e => e.Department);

            builder.Entity<Team>()
               .HasMany(d => d.Employees);

            // Customize the ASP.NET
            // Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Position> Positions { get; set; }

    }
}
