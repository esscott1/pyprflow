using System;
using Microsoft.EntityFrameworkCore;
//using Api.Database.Entity.Threats;
//using Api.Database.Entity;
//using Portal.Core;
using System.Threading.Tasks;
using System.Linq;
using pyprflow.Database.Entity;


namespace pyprflow.Database
{
    public class ApiContext : DbContext
    {
        public DbSet<BaseWorkflowItem> WorkflowDb { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

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
                    optionsBuilder.UseSqlServer(conn);
                    //   optionsBuilder.UseSqlite(conn, x => x.SuppressForeignKeyEnforcement());
                    break;
                default:
                    optionsBuilder.UseSqlServer(conn);
                    //  optionsBuilder.UseSqlite(conn, x => x.SuppressForeignKeyEnforcement());
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
