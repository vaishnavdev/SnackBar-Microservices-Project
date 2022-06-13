namespace SnackBar.Services.Email.Messages
{
    public class PaymentResponseMessage
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
