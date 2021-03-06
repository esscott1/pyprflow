﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace pyprflow.Api.Migrations
{
    public partial class mssql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowDb",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    DerivedType = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    SerializedObject = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDb", x => new { x.Name, x.DerivedType });
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    RelationshipId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                         .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Active = table.Column<bool>(nullable: false),
                    AssignedTo = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    NodeName = table.Column<string>(nullable: true),
                    PreviousNodeName = table.Column<string>(nullable: true),
                    Submitter = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TrackableName = table.Column<string>(nullable: true),
                    TransactionName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    WorkflowName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => x.RelationshipId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowDb");

            migrationBuilder.DropTable(
                name: "Relationships");
        }
    }
}
