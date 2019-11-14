using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace pyprSecure
{
    public class Startup
    {
        const string connectionString =
    @"Server=127.0.0.1\SQLEXPRESS2017;database=Test.IdentityServer4.EntityFramework;User Id = sa; Password=!!nimda1;";
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(builder =>
               builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                 .AddOperationalStore(options =>
                         options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                 .AddConfigurationStore(options =>
                        options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                 .AddAspNetIdentity<IdentityUser>()
                // .AddInMemoryClients(Clients.Get())
                //  .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                //  .AddInMemoryApiResources(Resources.GetApiResources())
                // .AddTestUsers(Users.Get())
                .AddDeveloperSigningCredential();
               
                

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));


      

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIdentityServer();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDbTestData(app);
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var testUser in Users.Get())
                    {
                        var identityUser = new IdentityUser(testUser.Username)
                        {
                            Id = testUser.SubjectId
                        };

                        userManager.CreateAsync(identityUser, "Password123!").Wait();
                        userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                    }
                }
            }
        }
    }
}