using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using pyprflow.Database;
using pyprflow.Database.Entity;

namespace pyprflow.Migrations
{
    [DbContext(typeof(ApiContext))]
    partial class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("pyprflow.Database.Entity.BaseWorkflowItem", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("DerivedType");

                    b.Property<bool>("Active");

                    b.Property<string>("SerializedObject");

                    b.HasKey("Name", "DerivedType");

                    b.ToTable("WorkflowDb");
                });

            modelBuilder.Entity("pyprflow.Database.Entity.Relationship", b =>
                {
                    b.Property<int>("RelationshipId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("AssignedTo");

                    b.Property<string>("NodeName");

                    b.Property<string>("Submitter");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("TrackableName");

                    b.Property<string>("TransactionName");

                    b.Property<int>("Type");

                    b.Property<string>("WorkflowName");

                    b.HasKey("RelationshipId");

                    b.ToTable("Relationships");
                });
        }
    }
}
