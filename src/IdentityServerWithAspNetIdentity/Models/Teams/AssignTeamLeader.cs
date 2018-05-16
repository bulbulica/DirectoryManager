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
        public Team Team { get; set; }
        public IEnumerable<Employee> Employees {get;set;}
        public int IdTeamLeader { get; set; }
    }
}
