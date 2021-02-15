using Microsoft.EntityFrameworkCore.Migrations;

namespace SemesterProjectManager.Data.Migrations
{
    public partial class AddListOfTasksInTopicModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Tasks_TaskId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_TaskId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Topics");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TopicId",
                table: "Tasks",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Topics_TopicId",
                table: "Tasks",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Topics_TopicId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TopicId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Topics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TaskId",
                table: "Topics",
                column: "TaskId",
                unique: true,
                filter: "[TaskId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Tasks_TaskId",
                table: "Topics",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
