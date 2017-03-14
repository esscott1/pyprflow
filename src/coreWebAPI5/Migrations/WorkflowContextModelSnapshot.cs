using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using workflow.Model;

namespace coreWebAPI5.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    partial class WorkflowContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
        }
    }
}
