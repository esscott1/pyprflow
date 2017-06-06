using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using pyprflow.Model;

namespace pyprflow.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    [Migration("20170606005710_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("pyprflow.Model.BaseWorkflowItem", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("DerivedType");

                    b.Property<string>("SerializedObject");

                    b.HasKey("Name", "DerivedType");

                    b.ToTable("WorkflowDb");
                });

            modelBuilder.Entity("pyprflow.Model.Relationship", b =>
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
