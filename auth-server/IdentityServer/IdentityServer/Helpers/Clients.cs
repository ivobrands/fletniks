﻿﻿using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.Helpers
{
    internal class Clients {
        private IConfigurationRoot _Configuration;

        public Clients(IConfigurationRoot Configuration)
		{
			_Configuration = Configuration;
		}

        public static IEnumerable<Client> Get()
		{
			var redirectUriDevelopment = "http://localhost:5000";
			var redirectUri = "https://fletniks.azurewebsites.net";
			return new List<Client> {
				new Client {
					ClientId = "fletnix",
					ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
					AllowedGrantTypes = GrantTypes.List(
						GrantType.Implicit,
						GrantType.ClientCredentials),
					RequireConsent = false,
					AllowAccessTokensViaBrowser = true,
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"role"
					},
					RedirectUris = new List<string> {redirectUri +"/signin-oidc"},
					PostLogoutRedirectUris = new List<string> {redirectUri }
				},
				new Client {
					ClientId = "fletnixDevelopment",
					ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
					AllowedGrantTypes = GrantTypes.List(
						GrantType.Implicit,
						GrantType.ClientCredentials),
					RequireConsent = false,
					AllowAccessTokensViaBrowser = true,
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"role"
					},
					RedirectUris = new List<string> {redirectUriDevelopment +"/signin-oidc"},
					PostLogoutRedirectUris = new List<string> {redirectUriDevelopment }
				}
			};
		}
    }
}