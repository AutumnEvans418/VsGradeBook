using Microsoft.EntityFrameworkCore.Migrations;

namespace Grader.Web.Migrations
{
    public partial class AddRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_CodeProjects_CodeProjectId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_CodeProjectId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CodeProjectId",
                table: "Submissions");

            migrationBuilder.AddColumn<int>(
                name: "SubmissionId1",
                table: "SubmissionFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ProjectId",
                table: "Submissions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles",
                column: "SubmissionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles",
                column: "SubmissionId1",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_CodeProjects_ProjectId",
                table: "Submissions",
                column: "ProjectId",
                principalTable: "CodeProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_CodeProjects_ProjectId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ProjectId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropColumn(
                name: "SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.AddColumn<int>(
                name: "CodeProjectId",
                table: "Submissions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_CodeProjectId",
                table: "Submissions",
                column: "CodeProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_CodeProjects_CodeProjectId",
                table: "Submissions",
                column: "CodeProjectId",
                principalTable: "CodeProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
