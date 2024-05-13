using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSW_Dusan_Markovic.Migrations
{
    public partial class AwardMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorAwards",
                columns: table => new
                {
                    AwardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfAwards = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorAwards", x => x.AwardId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorAwards");
        }
    }
}
