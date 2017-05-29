using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServer.Helpers;
using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ApiResource = IdentityServer4.Models.ApiResource;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AuthenticationServer.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace AuthenticationServer
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
			services.AddSingleton(Configuration);

            services.AddAuthentication();
			//services.AddTransient<IdentitySeedData>();

//			services.AddDbContext<authenticationDbContext>();
//            services.AddDbContext<fletnixContext>();
//
//			//services.AddDbContext<FLETNIXContext>(options =>
//				//options.UseSqlServer(Configuration["Database:Fletnix"]));
//
//			services.AddIdentity<IdentityUser, IdentityRole>(config =>
//				{
//					config.User.RequireUniqueEmail = true;
//					config.Password.RequiredLength = 5;
//					config.Password.RequireNonAlphanumeric = false;
//				})
//                .AddEntityFrameworkStores<authenticationDbContext>()
//				.AddDefaultTokenProviders();

			services.AddMvc();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
			services.AddIdentityServer()
			   .AddOperationalStore(
				   builder => builder.UseSqlServer(Configuration["Database:FletnixAuth"], options => options.MigrationsAssembly(migrationsAssembly)))
			   //.AddInMemoryClients(Clients.Get())
			   //.AddInMemoryIdentityResources(Resources.GetIdentityResources())
			   //.AddInMemoryApiResources(Resources.GetApiResources())
			   .AddConfigurationStore(
					builder => builder.UseSqlServer(Configuration["Database:FletnixAuth"], options => options.MigrationsAssembly(migrationsAssembly)))
			   //.AddTestUsers(Users.Get())
			   .AddAspNetIdentity<IdentityUser>()
			   .AddTemporarySigningCredential();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Database:FletnixAuth"]));
            services.AddIdentity<IdentityUser, IdentityRole>
                ()
                .AddEntityFrameworkStores<ApplicationDbContext>();

//			services.AddIdentityServer()
//                .AddInMemoryClients(Clients.Get())
//                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
//                .AddInMemoryApiResources(Resources.GetApiResources())
//                .AddTestUsers(Users.Get())
//                .AddTemporarySigningCredential();
            
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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

            app.UseIdentityServer();

            app.UseIdentity();

            app.UseMvcWithDefaultRoute();

        }
    }
}