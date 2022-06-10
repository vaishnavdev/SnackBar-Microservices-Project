using Microsoft.AspNetCore.Mvc;
using SnackBar.MessageBus;
using SnackBar.Services.ShoppingCartAPI.Messages;
using SnackBar.Services.ShoppingCartAPI.Models.Dto;
using SnackBar.Services.ShoppingCartAPI.Repository;

namespace SnackBar.Services.ShoppingCartAPI.Controllers
{
    
    public class CartAPIController : Controller
    {

        protected ResponseDto _response;

        private ICartRepository _repo;

        private IMessageBus _messageBus;

        private readonly ICouponRepository _couponRepository;

        public CartAPIController(ICartRepository repo, 
                                 IMessageBus messageBus,
                                 ICouponRepository couponRepository)
        {
            _repo = repo;
            this._response = new ResponseDto();
            this._messageBus = messageBus;
            this._couponRepository = couponRepository;
        }

        [HttpGet]
        [Route("/get-cart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _repo.GetCartByUserId(userId);
                _response.Result = cartDto;

            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("/add-cart")]
        public async Task<object> AddCart([FromBody]CartDto cartDto)
        {
            try
            {
                CartDto model = await _repo.AddOrUpdateCart(cartDto);
                _response.Result = cartDto;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut]
        [Route("/update-cart")]
        public async Task<object> UpdateCart([FromBody]CartDto cartDto)
        {
            try
            {
                CartDto model = await _repo.AddOrUpdateCart(cartDto);
                _response.Result = cartDto;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("/remove-cart")]
        public async Task<object> RemoveCart([FromBody]int cartId)
        {
            try
            {
                bool isSuccess = await _repo.RemoveFromCart(cartId);
                _response.Result = isSuccess;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("/apply-coupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                //after clicking on apply the coupon check the coupon is valid or not by communicating with Coupon API 
                //make a sync comm with Coupon API
                bool isSuccess = await _repo.ApplyCoupon(cartDto.CartHeader.UserId, 
                                                         cartDto.CartHeader.CouponCode);
                _response.Result = isSuccess;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("/remove-coupon")]
        public async Task<object> RemoveCoupon([FromBody] string UserId)
        {
            try
            {
                bool isSuccess = await _repo.RemoveCoupon(UserId);
                _response.Result = isSuccess;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("/checkout")]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDto checkoutHeaderDto)
        {
            try
            {
                CartDto cartDto = await _repo.GetCartByUserId(checkoutHeaderDto.UserId);
                if(cartDto == null)
                {
                    return BadRequest(string.Empty);
                }
                if (!string.IsNullOrEmpty(checkoutHeaderDto.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCoupon(checkoutHeaderDto.CouponCode);
                    if(checkoutHeaderDto.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess=false;
                        _response.Errors = new List<string>() { "Coupon price has changed ! please check the 'order total' and confirm"};
                        _response.DisplayMessage = "Coupon price has changed ! please check the 'order total' and confirm";
                        return _response;
                    }
                }
                checkoutHeaderDto.CartDetails = cartDto.CartDetails;

                //send message to topic, so that order microservice can take the message and process it 
                await _messageBus.PublishMessage(checkoutHeaderDto, "checkoutmessagetopic");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
