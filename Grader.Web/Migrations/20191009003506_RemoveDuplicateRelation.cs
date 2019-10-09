using Microsoft.EntityFrameworkCore.Migrations;

namespace Grader.Web.Migrations
{
    public partial class RemoveDuplicateRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropColumn(
                name: "SubmissionId1",
                table: "SubmissionFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmissionId1",
                table: "SubmissionFiles",
                nullable: true);

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
        }
    }
}
