using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AvtoNetScraper.Migrations
{
    public partial class AddedNotificationLogAndCarName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cars",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationsLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarId = table.Column<int>(nullable: false),
                    SentTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationsLog");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cars");
        }
    }
}
