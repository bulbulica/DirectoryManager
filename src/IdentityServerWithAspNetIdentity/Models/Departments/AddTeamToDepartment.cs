using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AddTeamToDepartment
    {
        [Required]
        public Department Department { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public int TeamId { get; set; }

    }
}
