using SnackBar.Services.ShoppingCartAPI.Models.Dto;

namespace SnackBar.Services.ShoppingCartAPI.Models
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
