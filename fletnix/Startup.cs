﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
using fletnix.Models;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace fletnix
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddScoped<IMovieRepsitory, MovieRepository>();
            services.AddScoped<IMovieCastRepository, MovieCastRepository>();
            services.AddScoped<ICustomerFeedbackRepository, CustomerFeedbackRepository>();

            services.AddSingleton(provider => Configuration);

            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "rediscache";
                options.Configuration = "13.81.213.106:6379,password=Y3IxavvW61lPp/RkbjdleoBk5kwET453cKd7Ru7XOZc=,ssl=false,abortConnect=False";
            });

            services.AddSession();

            services.AddDbContext<fletnixContext>();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "admin"));

                options.AddPolicy("customer", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "customer","admin"));

                options.AddPolicy("Financial", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Financial","admin"));
            });


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {          
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
                
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
            
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
				app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "cookie",
                CookieHttpOnly = true
            });

			app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
			{
                
                AuthenticationScheme = "oidc",
                SignInScheme = "cookie",
                Authority = "http://localhost:5002/",
                //Authority = "https://fletniksauthentication.azurewebsites.net/",
                RequireHttpsMetadata = false,
                ClientId = "fletnixDevelopment",
                //ClientId = "fletnix",
                //ResponseType = "code id_token",
                Scope = { "openid", "profile","role"},
                GetClaimsFromUserInfoEndpoint = true,
                AutomaticChallenge = true,
                AutomaticAuthenticate = true,
                ResponseType = "id_token",
                //SaveTokens = true,

				TokenValidationParameters = new TokenValidationParameters
				{
					NameClaimType = JwtClaimTypes.Name,
					RoleClaimType = JwtClaimTypes.Role,
				}


			});


            app.UseSession();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                   template: "{controller=Home}/{action=index}/{id?}");
            });
        }
    }
}
