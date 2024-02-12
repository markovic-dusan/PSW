using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSW_Dusan_Markovic.Migrations
{
    public partial class migracijaNovaaaaa3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Tours_TourId",
                table: "Interests");

            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Users_UserId",
                table: "Interests");

            migrationBuilder.DropIndex(
                name: "IX_Interests_TourId",
                table: "Interests");

            migrationBuilder.DropIndex(
                name: "IX_Interests_UserId",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Interests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourId",
                table: "Interests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Interests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interests_TourId",
                table: "Interests",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Interests_UserId",
                table: "Interests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Tours_TourId",
                table: "Interests",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Users_UserId",
                table: "Interests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
