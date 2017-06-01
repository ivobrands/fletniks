﻿﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using fletnix.Models;
 using IdentityModel;
 using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace fletnix.Data.Seeds
{
    public class IdentitySeedData
    {
        private FLETNIXContext _context;
        private UserManager<IdentityUser> _userManager;

        public IdentitySeedData(FLETNIXContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {



            foreach (var customer in _context.Customer.ToList())
            {
                if (await _userManager.FindByEmailAsync(customer.CustomerMailAddress) == null)
                {


                    var claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, customer.CustomerMailAddress),
                        new Claim(JwtClaimTypes.Role, "customer"),
                        new Claim(JwtClaimTypes.Name, customer.CustomerMailAddress)
                    };

                    var identityUser = new IdentityUser()
                    {
						UserName = customer.CustomerMailAddress,
						Email = customer.CustomerMailAddress
                    };

                    foreach (var claim in claims)
                    {
                        identityUser.Claims.Add(new IdentityUserClaim<string>
                        {
                            ClaimType = claim.Type,
                            ClaimValue = claim.Value
                        });
                    }

                    _userManager.CreateAsync(identityUser, "Welkom123!").Wait();


                    /*var user = new IdentityUser()
                    {
                        UserName = customer.CustomerMailAddress,
                        Email = customer.CustomerMailAddress
                    };

                    user.Claims.Add(new IdentityUserClaim<string>
                    {
                        ClaimType=ClaimTypes.Name,
                        ClaimValue=customer.CustomerMailAddress
                    });

                    user.Claims.Add(new IdentityUserClaim<string>
                    {
                        ClaimType=ClaimTypes.Email,
                        ClaimValue=customer.CustomerMailAddress
                    });*/

                    /*user.Roles.Add(new IdentityUserRole<string>()
                    {
                        RoleId = "customer",
                        UserId = customer.CustomerMailAddress
                    });



                    var newUsr = await _userManager.CreateAsync(user, "Welkom123");*/

                }
            }


            if (await _userManager.FindByEmailAsync("ivobrands@home.nl") == null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, "ivobrands@home.nl"),
                    new Claim(JwtClaimTypes.Role, "admin"),
                    new Claim(JwtClaimTypes.Name, "ivobrands@home.nl")
                };

                var identityUser = new IdentityUser("ivobrands@home.nl")
                {
					UserName = "ivobrands@home.nl",
					Email = "ivobrands@home.nl"
                };

                foreach (var claim in claims)
                {
                    identityUser.Claims.Add(new IdentityUserClaim<string>
                    {
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value,
                    });
                }

                _userManager.CreateAsync(identityUser, "ivobrands").Wait();
            }


        }
    }
}