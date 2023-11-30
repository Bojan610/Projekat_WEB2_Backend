using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat_Web2.Migrations
{
    public partial class migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DelivererId",
                table: "OrderProductDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelivererId",
                table: "OrderProductDetails");
        }
    }
}
