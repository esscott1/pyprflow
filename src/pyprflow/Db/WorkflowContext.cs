using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pyprflow;
using pyprflow.Model;
using pyprflow.Database;

namespace pyprflow.Db
{
	public class WorkflowContext : DbContext
	{
		public DbSet<BaseWorkflowItem> WorkflowDb { get; set; }
		public DbSet<Relationship> Relationships { get; set; }

		public WorkflowContext(DbContextOptions<ApiContext> options)
			: base(options) {
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
