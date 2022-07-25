using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat_Web2.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Orders_CurrentOrder_ConId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentOrder_ConId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CurrentOrder_ConId",
                table: "Users",
                newName: "CurrentOrderID_Con");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentOrderID_Con",
                table: "Users",
                column: "CurrentOrderID_Con",
                unique: true,
                filter: "[CurrentOrderID_Con] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Orders_CurrentOrderID_Con",
                table: "Users",
                column: "CurrentOrderID_Con",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Orders_CurrentOrderID_Con",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentOrderID_Con",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CurrentOrderID_Con",
                table: "Users",
                newName: "CurrentOrder_ConId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentOrder_ConId",
                table: "Users",
                column: "CurrentOrder_ConId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Orders_CurrentOrder_ConId",
                table: "Users",
                column: "CurrentOrder_ConId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
