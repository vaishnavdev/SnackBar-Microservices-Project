using Microsoft.AspNetCore.Mvc;
using SnackBar.Services.CouponAPI.Models.Dto;
using SnackBar.Services.CouponAPI.Repository;

namespace SnackBar.Services.CouponAPI.Controllers
{
    //[Route("api/coupons")]
    public class CouponAPIController : Controller
    {

        private readonly ICouponRepository _couponRepo;
        protected ResponseDto _response;


        public CouponAPIController(ICouponRepository repo)
        {
            _couponRepo = repo;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Route("/get-coupon/{couponCode}")]
        public async Task<object> GetCoupon(string couponCode)
        {
            try
            {
                CouponDto couponDto = await _couponRepo.GetCouponByCode(couponCode);
                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
