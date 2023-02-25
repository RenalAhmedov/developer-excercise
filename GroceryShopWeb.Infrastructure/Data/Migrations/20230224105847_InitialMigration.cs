using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryShopWeb.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Price" },
                values: new object[,]
                {
                    { "apple", 50.0 },
                    { "banana", 40.0 },
                    { "potato", 26.0 },
                    { "tomato", 30.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
