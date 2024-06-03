using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSW_Dusan_Markovic.Migrations
{
    public partial class MaliciousTrackerFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numberOfStrikes",
                table: "MaliciousTrackers",
                newName: "NumberOfStrikes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfStrikes",
                table: "MaliciousTrackers",
                newName: "numberOfStrikes");
        }
    }
}
