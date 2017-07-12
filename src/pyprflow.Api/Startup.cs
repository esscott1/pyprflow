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
using pyprflow.Workflow.Helpers;
using pyprflow.Workflow.Model;
using pyprflow.Api.Middleware;
using pyprflow.Workflow.Db;
using pyprflow.Database;
using System.Linq.Expressions;

namespace pyprflow.Api
{
    public class Startup
    {

        //internal static Dictionary<string, Action<DbContextOptionsBuilder>> _DbContextStrategy =
        //   new Dictionary<string, Action<DbContextOptionsBuilder>>()
        //   {
        //     { "test",  o => o.UseSqlServer(_conn, m => m.MigrationsAssembly("pyprflow")) },
        //     { "mssql2017", o => o.UseSqlServer(_conn, m => m.MigrationsAssembly("pyprflow")) },
        //     { "local", o => o.UseSqlServer(_conn, m => m.MigrationsAssembly("pyprflow")) },
        //     { "mssql", o => o.UseSqlServer(_conn, m => m.MigrationsAssembly("pyprflow")) },
        //     {"sqlite", o => o.UseSqlite(_conn, m => { m.SuppressForeignKeyEnforcement(); m.MigrationsAssembly("pyprflow"); }) }

        //   };
       // static string _conn;


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
            services.AddMvc();
			services.AddSingleton<IWorkflowRepository, WorkflowRepository>();
            string dbtype = Environment.GetEnvironmentVariable("pfdatabasetype");
            Console.WriteLine("OS is: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("pfdatabasetype ENV var is: " + dbtype);
            Console.WriteLine("is dbtype null? : "+ String.IsNullOrWhiteSpace(dbtype));
            if (string.IsNullOrWhiteSpace(dbtype))
                dbtype = "local";
            IDbProvider Iconn = new DbProviderFactory().Create(dbtype);
          
            Console.WriteLine("connection string used is: "+ Iconn.ConnectionString);
           
            services.AddDbContext<ApiContext>(Iconn.dbContext);
            
           
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
