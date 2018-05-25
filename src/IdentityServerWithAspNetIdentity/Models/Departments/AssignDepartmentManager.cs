using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AssignDepartmentManager
    {
        public Department Department { get; set; }
        public IEnumerable<Employee> Employees {get;set;}
        public int IdDepartmentManager { get; set; }
    }
}
