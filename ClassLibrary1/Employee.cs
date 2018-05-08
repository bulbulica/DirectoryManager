using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }
        public string CV { get; set; }
        public bool Active { get; set; }

        public virtual AssignedTeam AssignedTeam { get; set; }
        public virtual Team Team { get; set; }
        public virtual Department  Department { get; set; }
    }
}
