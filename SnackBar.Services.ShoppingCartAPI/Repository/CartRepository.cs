using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnackBar.Services.ShoppingCartAPI.DbContexts;
using SnackBar.Services.ShoppingCartAPI.Models;
using SnackBar.Services.ShoppingCartAPI.Models.Dto;

namespace SnackBar.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {

        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public CartRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CartDto> AddOrUpdateCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);
            //check if product exists in DB
            var prodInDb = await _context.Products
                .FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);
            if (prodInDb == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }
            //check if CartHeader is null or not
            var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
            if(cartHeaderFromDb == null)
            {
                //Add/Create Header and Details
                _context.CartHeaders.Add(cart.CartHeader);
               await  _context.SaveChangesAsync();
                //populate cart details using above inserted cartDetails id
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                //if CartHeader is not null //check if  cartdetails has same product
                var cartDetailsFromDb = await _context.CartDetails.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                if (cartDetailsFromDb == null)
                {
                    // create/add CartDetails
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //update count to cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count+= cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> ApplyCoupon(string UserId, string CouponCode)
        {
            var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == UserId);
            cartHeaderFromDb.CouponCode = CouponCode;
            _context.Update(cartHeaderFromDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCart(string UserId)
        {
            var CartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == UserId);
            if(CartHeaderFromDb != null)
            {
                _context.CartDetails
                    .RemoveRange(_context.CartDetails.Where(u => u.CartHeaderId == CartHeaderFromDb.CartHeaderId));
                _context.CartHeaders.Remove(CartHeaderFromDb);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDto> GetCartByUserId(string UserId)
        {
            Cart cart = new()
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == UserId)
            };
            cart.CartDetails = _context.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).Include(u => u.Product);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveCoupon(string UserId)
        {
            var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == UserId);
            cartHeaderFromDb.CouponCode = "";
            _context.Update(cartHeaderFromDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCart(int CartDetailsId)
        {
            try 
            {
                CartDetails cartDetails = await _context.CartDetails
                .FirstOrDefaultAsync(u => u.CartDetailsId == CartDetailsId);

                int totalCountOfCartItems = _context.CartDetails
                    .Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetails);
                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            } 

        }
    }
}
