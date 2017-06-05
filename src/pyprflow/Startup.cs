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
            services.AddDbContext<WorkflowContext>(options =>
                options.UseSqlite("Filename=./Repository.db", x => x.SuppressForeignKeyEnforcement()));

            //services.AddDbContext<WorkflowContext>(options =>
            //options.UseSqlServer(@"Server=10.0.0.25;Database=pyprflowlocaldb;User Id=sa;Password=!!Nimda1;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
			//app.ApplyUserKeyValidation();

            app.UseMvc();
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				serviceScope.ServiceProvider.GetService<WorkflowContext>().Database.Migrate();
				
			}
		}
    }
}
