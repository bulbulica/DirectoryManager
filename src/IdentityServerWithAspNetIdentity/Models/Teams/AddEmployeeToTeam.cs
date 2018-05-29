using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AddEmployeeToTeam
    {
        public Team Team { get; set; }
        public IEnumerable<Employee> Employees {get;set;}
        public int EmployeeId { get; set; }
    }
}
