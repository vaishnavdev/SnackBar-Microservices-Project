namespace SnackBar.Services.Email.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        public Task StartProcessorAsync();
        public Task StopProcessorAsync();
    }
}
