using SnackBar.Services.PaymentAPI.Messaging;

namespace SnackBar.Services.PaymentAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IAzureServiceBusConsumer messageConsumer { get; set; }  
        
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            messageConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopped.Register(OnStop);
            return app;
        }

        private static void OnStart()
        {
            messageConsumer.StartProcessorAsync();
        }

        private static void OnStop()
        {
            //stops the processing when application is stopped
            messageConsumer.StopProcessorAsync();
        }
    }
}
