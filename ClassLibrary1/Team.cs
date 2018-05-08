using System.Collections;

namespace IdentityServer.Domain
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IEnumerable Employees { get; set; }
        public virtual Employee Employee { get; set; }

        public int IdTeamLeader { get; set; }
        public virtual Employee TeamLeader { get; set; }
    }
}