using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pyprflow;
using pyprflow.Model;

namespace pyprflow.Db
{
	public class WorkflowContext : DbContext
	{
		public DbSet<BaseWorkflowItem> WorkflowDb { get; set; }
		public DbSet<Relationship> Relationships { get; set; }

		public WorkflowContext(DbContextOptions<WorkflowContext> options)
			: base(options) {
        }
       
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            Helpers.IConnectionString Iconn = null;
            Iconn = Helpers.ConnectionStringFactory.GetConnetionString();
            string conn = Iconn.ConnectionString();
            switch (Environment.GetEnvironmentVariable("pfdatabasetype"))
            {
                case "mssql":
                    optionsBuilder.UseSqlServer(conn);
                    break;
                case "mssql2017":
                    optionsBuilder.UseSqlServer(conn);
                    break;
                case null:
                    optionsBuilder.UseSqlite(conn, x => x.SuppressForeignKeyEnforcement());
                    break;
                default:
                    optionsBuilder.UseSqlite(conn, x => x.SuppressForeignKeyEnforcement());
                    break;


            }
          

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
