using SnackBar.Web.Models;

namespace SnackBar.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<T> GetCoupon<T>(string couponCode, string token = null);


    }
}
