using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryShopWeb.Infrastructure.Data.Migrations
{
    public partial class ChangesToSeedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Deals",
                keyColumn: "Id",
                keyValue: 2,
                column: "Arguments",
                value: "potato");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Deals",
                keyColumn: "Id",
                keyValue: 2,
                column: "Arguments",
                value: "tomato");
        }
    }
}
