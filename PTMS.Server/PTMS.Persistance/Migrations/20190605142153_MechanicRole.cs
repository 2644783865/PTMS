using Microsoft.EntityFrameworkCore.Migrations;
using PTMS.Persistance.Seeding;

namespace PTMS.Persistance.Migrations
{
    public partial class MechanicRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteIds",
                table: "AppUser",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql(UsersSql.MechanicRoleSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteIds",
                table: "AppUser");
        }
    }
}
