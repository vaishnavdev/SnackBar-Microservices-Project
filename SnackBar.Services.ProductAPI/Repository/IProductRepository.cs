using SnackBar.Services.ProductAPI.Model.Dto;

namespace SnackBar.Services.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> getProductById(int productId);
        Task<ProductDto> AddOrUpdateProduct(ProductDto product);
        Task<bool> DeleteProduct(int ProductId);
    }
}
