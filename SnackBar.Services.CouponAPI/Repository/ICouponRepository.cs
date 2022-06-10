using SnackBar.Services.CouponAPI.Models.Dto;

namespace SnackBar.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        public Task<CouponDto> GetCouponByCode(string CouponCode);
    }
}
