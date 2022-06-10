namespace SnackBar.Services.PaymentAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        public Task StartProcessorAsync();
        public Task StopProcessorAsync();
    }
}
