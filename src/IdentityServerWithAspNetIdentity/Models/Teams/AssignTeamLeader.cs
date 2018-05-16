using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AssignTeamLeader
    {
        [Required]
        public int Id { get; set; }

        public Team Team { get; set; }
        public IEnumerable<Employee> Employees {get;set;}
        public Employee TeamLeader { get; set; }
    }
}
