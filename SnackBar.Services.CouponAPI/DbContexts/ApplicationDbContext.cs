using Microsoft.EntityFrameworkCore;
using SnackBar.Services.CouponAPI.Models;

namespace SnackBar.Services.CouponAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 1,
                CouponCode = "10OFF",
                DiscountAmount = 10
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 2,
                CouponCode = "15OFF",
                DiscountAmount = 15
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 3,
                CouponCode = "20OFF",
                DiscountAmount = 20
            });
        }
    }
}
