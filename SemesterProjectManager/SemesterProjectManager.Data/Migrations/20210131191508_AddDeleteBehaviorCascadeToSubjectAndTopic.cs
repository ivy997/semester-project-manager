using Microsoft.EntityFrameworkCore.Migrations;

namespace SemesterProjectManager.Data.Migrations
{
    public partial class AddDeleteBehaviorCascadeToSubjectAndTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Topics_TopicId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Subjects_SubjectId",
                table: "Topics");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Topics_TopicId",
                table: "Tasks",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Subjects_SubjectId",
                table: "Topics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Topics_TopicId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Subjects_SubjectId",
                table: "Topics");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Topics_TopicId",
                table: "Tasks",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Subjects_SubjectId",
                table: "Topics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
