using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnackBar.Services.CouponAPI.DbContexts;
using SnackBar.Services.CouponAPI.Models.Dto;

namespace SnackBar.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public CouponRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }
        public async Task<CouponDto> GetCouponByCode(string CouponCode)
        {
            var couponFromDb = await _context.Coupons.FirstOrDefaultAsync(u => u.CouponCode == CouponCode);
            if(couponFromDb == null)
            {
                return null;
            }
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
