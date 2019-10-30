using Microsoft.EntityFrameworkCore.Migrations;

namespace Grader.Web.Migrations
{
    public partial class addPlagiarizeFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlagiarized",
                table: "Submissions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlagiarized",
                table: "Submissions");
        }
    }
}
