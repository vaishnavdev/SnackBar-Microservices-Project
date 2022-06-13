using Microsoft.EntityFrameworkCore;
using SnackBar.Services.Email.Models;

namespace SnackBar.Services.Email.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<EmailLog> EmailLogs { get; set; }
    }
}
