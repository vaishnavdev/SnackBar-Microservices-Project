using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnackBar.Services.ProductAPI.Migrations
{
    public partial class AddpProductsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Appetizer", "Tasty Samosa with onion and corn belended with fresh butter inside it.", "https://snackbarstorage.blob.core.windows.net/snackbar/14.jpg", "Samosa", 15.0 },
                    { 2, "Appetizer", "Tasty Paneer Tikka with onion and corn belended with fresh butter inside it.", "https://snackbarstorage.blob.core.windows.net/snackbar/12.jpg", "Paneer Tikka", 13.99 },
                    { 3, "Dessert", "Sweet and Chocolaty Pie rich in caramel and chocolate", "https://snackbarstorage.blob.core.windows.net/snackbar/11.jpg", "Choco Pie", 15.890000000000001 },
                    { 4, "Entree", "Hot and Saucy Pav Bhaji with Chana Curry", "https://snackbarstorage.blob.core.windows.net/snackbar/13.jpg", "Pav Bhaji", 25.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
