using System;
using Microsoft.EntityFrameworkCore;
//using Api.Database.Entity.Threats;
//using Api.Database.Entity;
//using Portal.Core;
using System.Threading.Tasks;
using System.Linq;
using pyprflow.Database.Entity;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace pyprflow.Database
{
    public class ApiContext : DbContext
    {
        public DbSet<BaseDbWorkFlowItem> WorkflowDb { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        //    modelBuilder.ForNpgsqlUseSerialColumns();
            modelBuilder.Entity<BaseDbWorkFlowItem>()
                .HasKey(w => new { w.Name, w.DerivedType });

        

            modelBuilder.Entity<Relationship>().Property(r => r.RelationshipId)
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .ValueGeneratedOnAdd();
            //.has
            //.HasKey(r => new { r.RelationshipId })
            
            

            modelBuilder.ForNpgsqlUseIdentityColumns();

            
        }
    }
}
