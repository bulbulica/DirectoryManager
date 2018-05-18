using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer
{
    public class AddTeam
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        //[Required]
        //aici trebuie modific cu un select pentru care departament e echipa respectiva
        public List<Department> Departments { get; set; }
        
        public Department Department { get; set; }

        public Employee TeamLeader { get; set; }
    }
}
