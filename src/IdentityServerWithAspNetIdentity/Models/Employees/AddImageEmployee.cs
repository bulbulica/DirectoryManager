// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer
{
    public class AddImageEmployee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IFormFile Image { set; get; }

        public string Error { get; set; }
    }
}