namespace SnackBar.Services.OrderAPI.Messages
{
    public class PaymentResponseMessage
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
    }
}
