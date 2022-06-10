
using SnackBar.Web.Models;

namespace SnackBar.Web.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int Id, string token);

        Task<T> CreateProductAsync<T>(ProductDto productDto, string token);
        Task<T> UpdateProductAsync<T>(ProductDto productDto, string token);
        Task<T> EvictProductAsync<T>(int id, string token);
    }
}
