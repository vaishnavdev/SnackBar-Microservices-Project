using SnackBar.Services.OrderAPI.Messages.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackBar.Services.OrderAPI.Messages.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }

        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }

        public int Count { get; set; }
    }
}
