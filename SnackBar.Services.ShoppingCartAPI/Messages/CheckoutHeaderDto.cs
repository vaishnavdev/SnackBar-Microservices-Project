using SnackBar.MessageBus;
using SnackBar.Services.ShoppingCartAPI.Models.Dto;

namespace SnackBar.Services.ShoppingCartAPI.Messages
{
    public class CheckoutHeaderDto : BaseMessage
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }

        public Double OrderTotal { get; set; }
        public Double DiscountTotal { get; set; }
        public string FirstName { get; set; }

        public String LastName { get; set; }

        public DateTime PickUpDateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonthYear { get; set; }

        public int CartTotalItems { get; set; }

        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
