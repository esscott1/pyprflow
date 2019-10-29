using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer;
using pyprflow.Workflow.Helpers;
using pyprflow.Workflow.Model;
using pyprflow.Api.Middleware;
using pyprflow.Database;
using System.Linq.Expressions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace pyprflow.Api
{
    public class Startup
    {

       
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration  Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddMvc(options => options.OutputFormatters.Add(new HtmlOutputFormatter()))
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            string dbtype = Environment.GetEnvironmentVariable("pfdatabasetype");
            string databasetype = Configuration["Db:Type"];
            if (string.IsNullOrWhiteSpace(dbtype))
                 dbtype = "local";
            //  dbtype = "postgres";
            dbtype = databasetype;
            pyprflow.Database.IDbProvider Iconn = new pyprflow.Database.DbProviderFactory().Create(dbtype);
            services.AddDbContext<ApiContext>(Iconn.dbContext,ServiceLifetime.Scoped);


            Console.WriteLine("OS is: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("pfdatabasetype ENV var is: " + dbtype);
            Console.WriteLine("is dbtype null? : " + String.IsNullOrWhiteSpace(dbtype));
            Console.WriteLine("connection string used is: " + Iconn.ConnectionString);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My Api", Version = "v1" });
            });
            
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //app.ApplyUserKeyValidation();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //Path.Combine(Directory.GetCurrentDirectory(), @"MyStaticFiles")),
            //    RequestPath = new PathString("/HTML")
            //});

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            // below is added to create the DB if it does not already exist.
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApiContext>().Database.Migrate();

            }
		}
    }
}
