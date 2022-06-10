using Microsoft.EntityFrameworkCore;
using SnackBar.Services.OrderAPI.DbContexts;
using SnackBar.Services.OrderAPI.Models;

namespace SnackBar.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public OrderRepository(DbContextOptions<ApplicationDbContext> options)
        {
            this._options = options;
        }
        public async Task<bool> SaveOrder(OrderHeader orderHeader)
        {
            try
            {
               var _context = new ApplicationDbContext(this._options);
                _context.OrderHeaders.Add(orderHeader);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {

            await using var _context = new ApplicationDbContext(this._options);
            var orderHeaderFromDb = await _context.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _context.SaveChangesAsync();
            }
        }
    }
}