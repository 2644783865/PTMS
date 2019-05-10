using Microsoft.EntityFrameworkCore.Migrations;
using PTMS.Persistance.Seeding;

namespace PTMS.Persistance.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var sql in UsersSql.SqlStatements)
            {
                migrationBuilder.Sql(sql);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
