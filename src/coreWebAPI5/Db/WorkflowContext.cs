using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace workflow.Model
{
	public class WorkflowContext : DbContext
	{
		public DbSet<BaseWorkflowItem> WorkflowDb { get; set; }
		public DbSet<Relationship> Relationships { get; set; }
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=./Repository.db", x => x.SuppressForeignKeyEnforcement());
				

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BaseWorkflowItem>()
				.HasKey(w => new { w.Name, w.DerivedType });
				
			modelBuilder.Entity<Relationship>()
				.HasKey(r => new {r.RelationshipId });
				
		}
	}

	
}
