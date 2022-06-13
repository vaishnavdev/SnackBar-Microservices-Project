using SnackBar.Services.Email.Messages;

namespace SnackBar.Services.Email.Repository
{
    public interface IEmailRepository
    {
        Task SendAndLogEmail(PaymentResponseMessage message);
    }
}
