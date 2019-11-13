using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyprSecure
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "apiclient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api"},
                      Claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim("fedorg","unique_Value")
                    },
                       AlwaysSendClientClaims = true

                },
                new Client {
                    ClientId = "oauthClient",
                    ClientName = "Example Client Credentials Client Application",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials, 
                    
                    ClientSecrets = new List<Secret> {
                        new Secret("superSecretPassword".Sha256())},
                    AllowedScopes = new List<string> {
                         IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "customAPI.read",
                        "api" },
                    Claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim("fedorg","unique_Value")
                    },
                    AlwaysSendClientClaims = true
                    
                    
                },
                new Client {
                    ClientId = "openIdConnectClient",
                    ClientName = "Example Implicit Client Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI.write"
                    },
                    RedirectUris = new List<string> {"https://localhost:44330/signin-oidc"},
                    PostLogoutRedirectUris = new List<string> { "https://localhost:44330" }
                }
            };
        }
    }
}
