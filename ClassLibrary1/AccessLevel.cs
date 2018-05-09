using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Domain
{
    class AccessLevel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string RoleName { get; set; }
    }
}
