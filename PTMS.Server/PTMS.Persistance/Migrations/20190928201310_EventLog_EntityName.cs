using Microsoft.EntityFrameworkCore.Migrations;

namespace PTMS.Persistance.Migrations
{
    public partial class EventLog_EntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityName",
                table: "EventLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityName",
                table: "EventLog");
        }
    }
}
