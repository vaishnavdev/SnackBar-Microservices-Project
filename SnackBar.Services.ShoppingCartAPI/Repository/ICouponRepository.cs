using SnackBar.Services.ShoppingCartAPI.Models.Dto;

namespace SnackBar.Services.ShoppingCartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
