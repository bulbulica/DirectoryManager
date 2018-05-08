// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer
{
    public class SingleEmployee
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }
        public bool Active { get; set; }
        public Team Team { get; set; }
        public Department Department { get; set; }
        public string Role { get; set; }
    }
}