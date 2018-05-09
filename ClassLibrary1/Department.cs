using System.Collections;

namespace IdentityServer.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable Teams { get; set; }
        public virtual Team Team { get; set; }

        public IEnumerable Employees { get; set; }
        public virtual Employee Employee { get; set; }

        public int IdDepartmentManager { get; set; }
        public virtual Employee DepartmentManager { get; set; }
    }
}