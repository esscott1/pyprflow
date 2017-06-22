using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer;
using pyprflow.Model;
using pyprflow.Middleware;
using pyprflow.Db;
using pyprflow.Database;

namespace pyprflow
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
			services.AddSingleton<IWorkflowRepository, WorkflowRepository>();

            ///hack hack hack... i've added this to the services collection but not sure how to explicitly access
            //todo:  need to add a helper class to build up querystring
            Helpers.IConnectionString Iconn = null;
            Iconn = Helpers.ConnectionStringFactory.GetConnetionString();
            string conn = Iconn.ConnectionString();
            Console.WriteLine("database type is " + Environment.GetEnvironmentVariable("pfdatabasetype"));
            Console.WriteLine("dASPNETCORE_ENVIRONMENT is " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            switch (Environment.GetEnvironmentVariable("pfdatabasetype"))
            {
                case "mssql":
                //case ("mssql"): 
                    services.AddDbContext<ApiContext>(options =>
                       options.UseSqlServer(conn));
                    break;
                case "mssql2017":
                    services.AddDbContext<ApiContext>(options =>
                        options.UseSqlServer(conn,
                        b => b.MigrationsAssembly("pyprflow")));
                    break;
                case null:
                    services.AddDbContext<ApiContext>(options =>
                       options.UseSqlite(conn, x => x.SuppressForeignKeyEnforcement()));
                    break;
                default:
                    services.AddDbContext<ApiContext>(options =>
                        options.UseSqlite(conn, x => x.SuppressForeignKeyEnforcement()));
                    break;


            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
			//app.ApplyUserKeyValidation();

            app.UseMvc();
            
            // below is added to create the DB if it does not already exist.
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

               
                serviceScope.ServiceProvider.GetService<ApiContext>().Database.Migrate();

            }
		}
    }
}
