using Newtonsoft.Json;
using SnackBar.Services.ShoppingCartAPI.Models.Dto;

namespace SnackBar.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44350/get-coupon/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
