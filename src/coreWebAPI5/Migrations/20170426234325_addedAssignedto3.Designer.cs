using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using workflow.Model;

namespace coreWebAPI5.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    [Migration("20170426234325_addedAssignedto3")]
    partial class addedAssignedto3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("workflow.Model.BaseWorkflowItem", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("DerivedType");

                    b.Property<string>("SerializedObject");

                    b.HasKey("Name", "DerivedType");

                    b.ToTable("WorkflowDb");
                });

            modelBuilder.Entity("workflow.Model.Relationship", b =>
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
