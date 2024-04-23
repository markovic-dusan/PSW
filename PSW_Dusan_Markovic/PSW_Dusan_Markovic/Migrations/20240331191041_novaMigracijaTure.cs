using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSW_Dusan_Markovic.Migrations
{
    public partial class novaMigracijaTure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchieved",
                table: "Tours",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchieved",
                table: "Tours");
        }
    }
}
