using SnackBar.Services.ShoppingCartAPI.Models.Dto;

namespace SnackBar.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
       Task<CartDto> GetCartByUserId(string UserId);
        Task<CartDto> AddOrUpdateCart(CartDto cartDto);
        Task<bool> RemoveFromCart(int CartDetailsId);
        Task<bool> ClearCart(string UserId);
        Task<bool> ApplyCoupon(string UserId, string CouponCode);
        Task<bool> RemoveCoupon(string UserId);

    }
}
