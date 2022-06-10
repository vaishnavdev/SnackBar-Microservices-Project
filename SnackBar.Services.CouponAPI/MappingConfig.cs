using AutoMapper;
using SnackBar.Services.CouponAPI.Models;
using SnackBar.Services.CouponAPI.Models.Dto;

namespace SnackBar.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<CouponDto,Coupon>().ReverseMap();
                //or
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });
            return mappingConfig;
        }
    }
}
