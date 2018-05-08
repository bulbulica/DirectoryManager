using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Core.Shared.Models
{
    public class LogoutContext
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string SignOutIFrameUrl { get; set; }

    }
}
