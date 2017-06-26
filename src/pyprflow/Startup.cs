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
           // var conn = "Filename=./Repository.db";
            // Add framework services.
            
            services.AddMvc();
			services.AddSingleton<IWorkflowRepository, WorkflowRepository>();
            string dbtype = Environment.GetEnvironmentVariable("pfdatabasetype");
            Console.WriteLine("OS is: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("pfdatabasetype ENV var is: " + dbtype);
            Console.WriteLine("is dbtype null? : "+ String.IsNullOrWhiteSpace(dbtype));
          //  Console.WriteLine("Count of dbtype? : " + dbtype.Count());
            Helpers.IDbProvider Iconn = new Helpers.DbProviderFactory().Create(dbtype);
            var conn = Iconn.ConnectionString;
            Console.WriteLine("connection string used is: "+ Iconn.ConnectionString);

            switch (dbtype)
            {
                case "mssql":
                    services.AddDbContext<ApiContext>(o => o.UseSqlServer(
                        conn, m => m.MigrationsAssembly("pyprflow"))
                    );
                    break;
                case "mssql2017":
                    services.AddDbContext<ApiContext>(o => o.UseSqlServer(
                        conn, m => m.MigrationsAssembly("pyprflow"))
                    );
                    break;
                case "local":
                    services.AddDbContext<ApiContext>(o => o.UseSqlServer(
                        conn, m => m.MigrationsAssembly("pyprflow"))
                        );
                    break;
                case null:
                    // services.AddDbContext<ApiContext>(o => o.UseSqlServer(
                    //    conn, m => m.MigrationsAssembly("pyprflow"))
                    //);
                    services.AddDbContext<ApiContext>(o => o.UseSqlite(conn,
                         x =>
                         {
                             x.SuppressForeignKeyEnforcement();
                             x.MigrationsAssembly("pyprflow");
                         }));
                    break;
                default: // this is what the CLI when running dotnet ef migration thinks a not included env var is
                         // services.AddDbContext<ApiContext>(o => o.UseSqlServer(
                         //    conn, m => m.MigrationsAssembly("pyprflow"))
                         //);
                    services.AddDbContext<ApiContext>(o => o.UseSqlite(conn,
                        x =>
                        {
                            x.SuppressForeignKeyEnforcement();
                            x.MigrationsAssembly("pyprflow");
                        }));
                    break;
            }
           

            //services.AddDbContext<ApiContext>(options => options.UseSqlServer(
            //     "Server=127.0.0.1,2250;Database=testcomponentdb;User Id=sa;Password=!!nimda1;",
            //    b => b.MigrationsAssembly("pyprflow"))
            //    ); // i should send in DbContextOptionsBuidler here and remove them for the 
            // ApiContext class.  move the connection string factory back to pyprflow.
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
