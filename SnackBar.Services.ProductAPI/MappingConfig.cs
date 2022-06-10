using AutoMapper;
using SnackBar.Services.ProductAPI.Model;
using SnackBar.Services.ProductAPI.Model.Dto;

namespace SnackBar.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<ProductDto,Product>().ReverseMap();
                //or
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
            });
            return mappingConfig;
        }
    }
}
