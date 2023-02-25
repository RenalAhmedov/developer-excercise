using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryShopWeb.Infrastructure.Data.Migrations
{
    public partial class DealEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "apple");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "banana");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "potato");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "tomato");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Arguments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Deals",
                columns: new[] { "Id", "Arguments", "Type" },
                values: new object[,]
                {
                    { 1, "apple, banana, tomato", "2 for 3" },
                    { 2, "tomato", "buy 1 get 1 for half price" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "aaaple", 50.0 },
                    { 2, "banana", 40.0 },
                    { 3, "tomato", 30.0 },
                    { 4, "potato", 26.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Name");
        }
    }
}
