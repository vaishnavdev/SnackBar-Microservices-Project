using SnackBar.Web.Models;

namespace SnackBar.Web.Services.IServices
{
    public class ProductService : BaseService, IProductService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
        {
           return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = productDto,
                Url = StaticDetails.ProductAPIBase+ "/add-product",
                AccessToken = token,
            });
        }

        public async Task<T> EvictProductAsync<T>(int id, string token)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.ProductAPIBase + "/erase-product/"+id,
                AccessToken = token,
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/get-all-products",
                AccessToken = token,
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int Id, string token)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/get-product/"+Id,
                AccessToken = token,
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.sendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = productDto,
                Url = StaticDetails.ProductAPIBase + "/modify-product",
                AccessToken = token,
            });
        }
    }
}
