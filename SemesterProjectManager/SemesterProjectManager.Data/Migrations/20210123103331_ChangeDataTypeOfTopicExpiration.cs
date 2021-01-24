using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SemesterProjectManager.Data.Migrations
{
    public partial class ChangeDataTypeOfTopicExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDuration",
                table: "Topics");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Topics",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Topics");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpirationDuration",
                table: "Topics",
                type: "time",
                nullable: true);
        }
    }
}
