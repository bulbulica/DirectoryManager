﻿using System.Collections;
using System.Collections.Generic;

namespace IdentityServer.Domain
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } 

        public int TeamLeaderId { get; set; }
        public virtual Employee TeamLeader { get; set; }
    }
}