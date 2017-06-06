using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pyprflow;

namespace pyprflow.Model
{
	public class WorkflowContext : DbContext
	{
		public DbSet<BaseWorkflowItem> WorkflowDb { get; set; }
		public DbSet<Relationship> Relationships { get; set; }
        private readonly DatabaseSettings _databaseSettings;

		public WorkflowContext(DbContextOptions<WorkflowContext> options)
			: base(options) {
           // _databaseSettings = dbsettings.Value;
        }
        //public WorkflowContext()
        //{
        //}

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            switch (Environment.GetEnvironmentVariable("DatabaseType").ToLower())
            {
                case ("mssql"):
                    optionsBuilder.UseSqlServer("Server = EricLaptop\\DEV2014; Database = pyprflowlocaldb; User Id = sa; Password = !!nimda;");
                    break;
                case ("mssql2017"):
                    optionsBuilder.UseSqlServer("Server = 10.0.0.25; Database = pyprflowlocaldb; User Id = sa; Password = !!Nimda1;");
                    break;
                default:
                    optionsBuilder.UseSqlite("Filename=./Repository.db", x => x.SuppressForeignKeyEnforcement());
                    break;


            }
            // 
            // optionsBuilder.UseSqlServer("Server = 10.0.0.25; Database = pyprflowlocaldb; User Id = sa; Password = !!Nimda1;");
            //optionsBuilder.UseSqlServer("Server = EricLaptop\\DEV2014; Database = pyprflowlocaldb; User Id = sa; Password = !!nimda;");
            //Console.WriteLine("Database Port is : " + _databaseSettings.Port);
            //this.Database.Migrate();

        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BaseWorkflowItem>()
				.HasKey(w => new { w.Name, w.DerivedType });

            modelBuilder.Entity<Relationship>()
                .HasKey(r => new { r.RelationshipId });
          
				
		}
	}

	
}
