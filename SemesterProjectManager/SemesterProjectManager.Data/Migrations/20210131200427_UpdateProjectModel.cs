using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SemesterProjectManager.Data.Migrations
{
    public partial class UpdateProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Projects",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Projects");
        }
    }
}
