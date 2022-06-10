using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnackBar.Services.ProductAPI.Data;
using SnackBar.Services.ProductAPI.Model;
using SnackBar.Services.ProductAPI.Model.Dto;
using System.Collections.Generic;

namespace SnackBar.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {

        protected readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductDto> AddOrUpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<ProductDto,Product>(productDto);
            if(product.ProductId > 0)
            {
                _context.Products.Update(product);
            }
            else
            {
               _context.Products.Add(product); 
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<Product,ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int ProductId)
        {
            bool isDeleted = false;
            try
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == ProductId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (product == null)
                {
                    isDeleted = false;
                    return isDeleted;
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                isDeleted = true;
                return isDeleted;
            }
            catch (Exception ex)
            {
                isDeleted=false;
                return isDeleted;
            }
        }

        public async Task<ProductDto> getProductById(int productId)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Product product = await _context.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> productList = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productList);
        }
    }
}
