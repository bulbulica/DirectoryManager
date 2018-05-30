using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.API
{
    [Produces("application/json")]
    [Authorize]
    public class APIController : Controller
    {
        [HttpGet]
        public IActionResult GetEmployees ()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

    }
}