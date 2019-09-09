using Microsoft.EntityFrameworkCore.Migrations;

namespace PTMS.Persistance.Migrations
{
    public partial class IsEndingStationFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Control",
                table: "BS_ROUTE                       ",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Control",
                table: "BS_ROUTE                       ");
        }
    }
}
