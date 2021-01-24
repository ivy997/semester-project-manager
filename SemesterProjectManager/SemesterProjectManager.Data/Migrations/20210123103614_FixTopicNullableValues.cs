using Microsoft.EntityFrameworkCore.Migrations;

namespace SemesterProjectManager.Data.Migrations
{
    public partial class FixTopicNullableValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Topics_ProjectId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_TaskId",
                table: "Topics");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Topics",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Topics",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ProjectId",
                table: "Topics",
                column: "ProjectId",
                unique: true,
                filter: "[ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TaskId",
                table: "Topics",
                column: "TaskId",
                unique: true,
                filter: "[TaskId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Topics_ProjectId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_TaskId",
                table: "Topics");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Topics",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Topics",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ProjectId",
                table: "Topics",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TaskId",
                table: "Topics",
                column: "TaskId",
                unique: true);
        }
    }
}
