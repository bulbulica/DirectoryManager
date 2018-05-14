// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer
{
    public class EditEmployee
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Picture { get; set; }
        public string CV { get; set; }
        public bool Active { get; set; }

        [Required]
        public string Position { get; set; }

        public IEnumerable<Position> AllPositions { get; set; }

        public Team Team { get; set; }

        public string Department { get; set; }

        public IEnumerable<Department> AllDepartments { get; set; }
    }
}