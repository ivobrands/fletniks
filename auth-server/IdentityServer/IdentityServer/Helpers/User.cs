using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace IdentityServer.Helpers
{
    internal class Users {
        public static List<TestUser> Get() {
            return new List<TestUser> {
                new TestUser {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "ivobrands",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "ivobrands@home.nl"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.Name, "ivobrands")
                    }
                },
                new TestUser {
                    SubjectId = "5BE86359-434B-073C-AD2D-A3932222DABE",
                    Username = "brandsivo",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "brands33@hotmail.com"),
                        new Claim(JwtClaimTypes.Role, "FinancialManager"),
                        new Claim(JwtClaimTypes.Name, "brandsivo")
                    }
                }
            };
        }
    }
}