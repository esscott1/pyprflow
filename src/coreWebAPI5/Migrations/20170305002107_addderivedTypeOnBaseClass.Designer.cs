using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using workflow.Model;

namespace coreWebAPI5.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    [Migration("20170305002107_addderivedTypeOnBaseClass")]
    partial class addderivedTypeOnBaseClass
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("workflow.Model.WorkflowItem", b =>
                {
                    b.Property<int>("WorkflowItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DerivedType");

                    b.Property<string>("SerializedObject");

                    b.HasKey("WorkflowItemId");

                    b.ToTable("WorkflowDb");
                });
        }
    }
}
