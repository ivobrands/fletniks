using System.Collections.Generic;
using System.Security.Cryptography;
using IdentityServer4;
using IdentityServer4.Models;

namespace AuthenticationServer.Helpers
{
    internal class Clients {
        public static IEnumerable<Client> Get() {
            return new List<Client> {
                new Client {
                    ClientId = "Fletnix",
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
                    RedirectUris = new List<string> {"http://localhost:5000/signin-oidc"},
                    PostLogoutRedirectUris = new List<string> {"http://localhost:5000"}
                }
            };


        }
    }
}
