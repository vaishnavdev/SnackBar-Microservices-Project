
namespace PaymentProcessor
{
    public class ProcessPayment : IProcessPayment
    {
        public bool PaymentProcessor()
        {
            //implement custom logic and get card details etc..use PCI compliants
            return true;
        }
    }
}