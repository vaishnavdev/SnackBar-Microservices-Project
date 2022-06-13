using Microsoft.EntityFrameworkCore;
using SnackBar.Services.Email.DbContexts;
using SnackBar.Services.Email.Messages;
using SnackBar.Services.Email.Models;

namespace SnackBar.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public EmailRepository(DbContextOptions<ApplicationDbContext> options)
        {
            this._options = options;
        }

        public async Task SendAndLogEmail(PaymentResponseMessage message)
        {
            //implement and email sender and send email using it
            EmailLog emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been placed successfully."
            };
            await using var _context = new ApplicationDbContext(this._options);
            _context.EmailLogs.Add(emailLog);   
            _context.SaveChanges();
        }
    }
}