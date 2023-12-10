using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTen.DAL.Migrations
{
    public partial class addednewfieldtotraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trainings");
        }
    }
}
