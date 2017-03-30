﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using workflow.Model;

namespace coreWebAPI5.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    [Migration("20170329145601_active")]
    partial class active
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

                    b.Property<string>("NodeName");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("TrackableName");

                    b.Property<string>("TransactionName");

                    b.Property<string>("WorkflowName");

                    b.HasKey("RelationshipId");

                    b.ToTable("Relationships");
                });
        }
    }
}