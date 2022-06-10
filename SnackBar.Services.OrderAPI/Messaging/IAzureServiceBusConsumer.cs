namespace SnackBar.Services.OrderAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        public Task StartProcessorAsync();
        public Task StopProcessorAsync();
    }
}
