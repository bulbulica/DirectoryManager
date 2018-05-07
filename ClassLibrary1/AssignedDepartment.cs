using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Domain
{
    class AssignedDepartment
    {
        public int Id { get; set; }

        public int IdDepartment { get; set; }
        public virtual Department Department { get; set; }

        public int IdTeam { get; set; }
        public virtual Team Team { get; set; }

    }
}
