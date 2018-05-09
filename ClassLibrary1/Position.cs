using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Domain
{
    public class Position
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int AccessLevel { get; set; }
        public string Description { get; set; }
    }
}
