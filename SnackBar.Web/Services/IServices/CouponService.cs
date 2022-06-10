using SnackBar.Web.Models;

namespace SnackBar.Web.Services.IServices
{
    public class CouponService :BaseService, ICouponService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        //ctor
        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T> GetCoupon<T>(string couponCode, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/get-coupon/" + couponCode,
                AccessToken = token,
            });
        }
    }
}
