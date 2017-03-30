﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace coreWebAPI5.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowDb",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    DerivedType = table.Column<string>(nullable: false),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    NodeName = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TrackableName = table.Column<string>(nullable: true),
                    TransactionName = table.Column<string>(nullable: true),
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