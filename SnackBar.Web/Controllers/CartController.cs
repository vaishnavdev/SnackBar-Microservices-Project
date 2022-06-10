using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SnackBar.Web.Models;
using SnackBar.Web.Services.IServices;

namespace SnackBar.Web.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService,
                              ICartService cartService,
                              ICouponService couponService)
        {
            this._cartService = cartService;
            this._productService = productService;
            this._couponService = couponService;  
        }
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoOfLoggedInUser());
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoOfLoggedInUser());
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                //var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.Checkout<ResponseDto>(cartDto.CartHeader, accessToken);
                if (!response.IsSuccess)
                {
                    TempData["Error"] = response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }

                TempData["Name"] = cartDto.CartHeader.FirstName + " " + cartDto.CartHeader.LastName;
                TempData["OrderTotal"] = cartDto.CartHeader.OrderTotal.ToString();
                TempData["Discount"] = cartDto.CartHeader.DiscountTotal.ToString();
                TempData["PhoneNumber"] = cartDto.CartHeader.Phone;
                TempData["Email"] = cartDto.CartHeader.Email;
                
                //foreach(var product in cartDto.CartDetails)
                //{
                //    TempData["Name"] = product.Product.Name;
                //    TempData["CostPerUnit"] = product.Product.Price;
                //}
                    
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                return View(cartDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCoupon<ResponseDto>(cartDto, accessToken);

            //before applying the coupon check the coupon is valid or not by communicating with Coupon API  and 
            //display an error message

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCoupon<ResponseDto>(cartDto.CartHeader.UserId, 
                                                                        accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDto> LoadCartDtoOfLoggedInUser()
        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(UserId, accessToken);

            CartDto cartDto = new CartDto();
            if(response != null && response.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            if(cartDto.CartHeader != null)
            {
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);
                    if (coupon != null && coupon.IsSuccess && coupon.Result!=null)
                    {
                        var couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Result));
                        cartDto.CartHeader.DiscountTotal = couponDto.DiscountAmount;
                    }
                }
                foreach(var detail in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }
            return cartDto;
        }

    }
}
