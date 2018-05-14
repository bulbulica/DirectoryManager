using IdentityServer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AllTeams
    {
        public IEnumerable<Team> Teams { get; set; }
    }
}
