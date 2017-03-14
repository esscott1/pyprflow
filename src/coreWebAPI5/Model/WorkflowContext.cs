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
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=./Repository.db");
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BaseWorkflowItem>()
				.HasKey(w => new { w.Name, w.DerivedType });
		}
	}

	
}
