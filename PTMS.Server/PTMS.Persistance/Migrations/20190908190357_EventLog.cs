using System;
using EntityFrameworkCore.FirebirdSql.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PTMS.Persistance.Migrations
{
    public partial class EventLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Event = table.Column<string>(maxLength: 30, nullable: false),
                    EntityType = table.Column<string>(maxLength: 20, nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventLogField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    EventLogId = table.Column<int>(nullable: false),
                    FieldName = table.Column<string>(maxLength: 30, nullable: false),
                    OldFieldValue = table.Column<string>(maxLength: 150, nullable: true),
                    NewFieldValue = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventLogId",
                        column: x => x.EventLogId,
                        principalTable: "EventLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_TimeStamp",
                table: "EventLog",
                column: "TimeStamp");

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_UserId",
                table: "EventLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogField_EventLogId",
                table: "EventLogField",
                column: "EventLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogField");

            migrationBuilder.DropTable(
                name: "EventLog");
        }
    }
}
