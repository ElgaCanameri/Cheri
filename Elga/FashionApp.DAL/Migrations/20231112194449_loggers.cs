using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionApp.DAL.Migrations
{
    public partial class loggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "AuditLogs");
        }
    }
}
