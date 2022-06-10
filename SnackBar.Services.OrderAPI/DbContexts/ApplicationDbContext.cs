using Microsoft.EntityFrameworkCore;
using SnackBar.Services.OrderAPI.Models;

namespace SnackBar.Services.OrderAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
