using SnackBar.Services.OrderAPI.Models;

namespace SnackBar.Services.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> SaveOrder(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
    }
}
