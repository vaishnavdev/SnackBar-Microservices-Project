using SnackBar.Web.Models;

namespace SnackBar.Web.Services.IServices
{
    public class CartService : BaseService, ICartService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        //ctor
        public CartService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/add-cart",
                AccessToken = token,
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/apply-coupon",
                AccessToken = token,
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartHeader,
                Url = StaticDetails.ShoppingCartAPIBase + "/checkout",
                AccessToken = token,
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ShoppingCartAPIBase + "/get-cart/" + userId,
                AccessToken = token,
            });
        }

        public async Task<T> RemoveCoupon<T>(string userId, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = userId,
                Url = StaticDetails.ShoppingCartAPIBase + "/remove-coupon",
                AccessToken = token,
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartId,
                Url = StaticDetails.ShoppingCartAPIBase + "/remove-cart",
                AccessToken = token,
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/update-cart",
                AccessToken = token,
            });
        }
    }
}
