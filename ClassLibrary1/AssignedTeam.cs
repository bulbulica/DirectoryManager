namespace IdentityServer.Domain
{
    public class AssignedTeam
    {
        public int Id { get; set; }

        public int IdTeam { get; set; }
        public virtual Team Team { get; set; }

        public int IdEmployee { get; set; }
        public virtual Employee Employee { get; set; }
    }
}