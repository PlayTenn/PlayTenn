using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTen.DAL.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TennisLevels_TennisLevelId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_AspNetUsers_UserId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_AspNetUsers_UserId",
                table: "Trainings");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TennisLevels_TennisLevelId",
                table: "AspNetUsers",
                column: "TennisLevelId",
                principalTable: "TennisLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_AspNetUsers_UserId",
                table: "Tournaments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_AspNetUsers_UserId",
                table: "Trainings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TennisLevels_TennisLevelId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_AspNetUsers_UserId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_AspNetUsers_UserId",
                table: "Trainings");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TennisLevels_TennisLevelId",
                table: "AspNetUsers",
                column: "TennisLevelId",
                principalTable: "TennisLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_AspNetUsers_UserId",
                table: "Tournaments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_AspNetUsers_UserId",
                table: "Trainings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
