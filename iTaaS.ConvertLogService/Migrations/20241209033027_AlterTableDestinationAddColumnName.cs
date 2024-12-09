using Microsoft.EntityFrameworkCore.Migrations;

namespace iTaaS.ConvertLogService.Migrations
{
    public partial class AlterTableDestinationAddColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Destinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Destinations");
        }
    }
}
