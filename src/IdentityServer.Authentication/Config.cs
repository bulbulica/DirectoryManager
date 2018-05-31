// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Authentication
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("GetEmployees", "Get Employees")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "EvaluationManagerClient",
                    ClientName = "Evaluation Manager Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("Pass123$".Sha256())
                    },                   
                    RedirectUris = { "http://localhost:51755/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:51755/" },
                    FrontChannelLogoutUri =  "http://localhost:51755/signout-oidc",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "GetEmployees"
                    },
                    AllowOfflineAccess = true
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,                    
                    RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("Pass123$".Sha256())
                    },                    
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/" },
                    FrontChannelLogoutUri =  "http://localhost:5002/signout-oidc",

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "GetEmployees"
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "EvaluationApp",
                    ClientName = "Evaluation Application",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("Pass123$".Sha256())
                    },
                    RedirectUris = { "http://localhost:51625/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:51625/" },
                    FrontChannelLogoutUri =  "http://localhost:51625/signout-oidc",

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "GetEmployees"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}