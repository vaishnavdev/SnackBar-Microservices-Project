using Microsoft.EntityFrameworkCore;
using SnackBar.Services.ProductAPI.Model;

namespace SnackBar.Services.ProductAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
          
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Samosa",
                Price = 15,
                Description = "Tasty Samosa with onion and corn belended with fresh butter inside it.",
                ImageUrl = "https://snackbarstorage.blob.core.windows.net/snackbar/14.jpg",
                CategoryName = "Appetizer"
            });

            builder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = "Tasty Paneer Tikka with onion and corn belended with fresh butter inside it.",
                ImageUrl = "https://snackbarstorage.blob.core.windows.net/snackbar/12.jpg",
                CategoryName = "Appetizer"
            });

            builder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Choco Pie",
                Price = 15.89,
                Description = "Sweet and Chocolaty Pie rich in caramel and chocolate",
                ImageUrl = "https://snackbarstorage.blob.core.windows.net/snackbar/11.jpg",
                CategoryName = "Dessert"
            });

            builder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Pav Bhaji",
                Price = 25,
                Description = "Hot and Saucy Pav Bhaji with Chana Curry",
                ImageUrl = "https://snackbarstorage.blob.core.windows.net/snackbar/13.jpg",
                CategoryName = "Entree"
            });
        }
    }
}
