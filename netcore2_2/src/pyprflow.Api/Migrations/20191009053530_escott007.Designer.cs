﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using pyprflow.Database;

namespace pyprflow.Api.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20191009053530_escott007")]
    partial class escott007
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("pyprflow.DbEntity.BaseDbWorkFlowItem", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("DerivedType");

                    b.Property<bool>("Active");

                    b.Property<bool>("Deleted");

                    b.Property<string>("SerializedObject");

                    b.HasKey("Name", "DerivedType");

                    b.ToTable("WorkflowDb");
                });

            modelBuilder.Entity("pyprflow.DbEntity.Relationship", b =>
                {
                    b.Property<int>("RelationshipId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("AssignedTo");

                    b.Property<string>("Comment");

                    b.Property<bool>("Deleted");

                    b.Property<string>("NodeName");

                    b.Property<string>("PreviousNodeName");

                    b.Property<string>("Submitter");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("TrackableName");

                    b.Property<string>("TransactionName");

                    b.Property<int>("Type");

                    b.Property<string>("WorkflowName");

                    b.HasKey("RelationshipId");

                    b.ToTable("Relationships");
                });
#pragma warning restore 612, 618
        }
    }
}
