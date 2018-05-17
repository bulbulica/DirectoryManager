using System.Collections;
using System.Collections.Generic;

namespace IdentityServer.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        
    }
}