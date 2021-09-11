using Microsoft.EntityFrameworkCore.Migrations;

namespace YellowPages.Database.Migrations
{
    public partial class ReportNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportPath",
                table: "Report",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportPath",
                table: "Report");
        }
    }
}
