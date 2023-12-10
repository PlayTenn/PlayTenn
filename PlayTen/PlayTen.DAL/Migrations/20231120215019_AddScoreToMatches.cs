using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTen.DAL.Migrations
{
    public partial class AddScoreToMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Score",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Matches");
        }
    }
}
