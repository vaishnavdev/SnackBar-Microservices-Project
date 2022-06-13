using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SnackBar.MessageBus;
using SnackBar.Services.Email.Messages;
using SnackBar.Services.Email.Repository;
using System.Text;

namespace SnackBar.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly EmailRepository _emailRepo;
        private readonly string subscriptionEmail;
        private readonly string serviceBusConnectionString;

        //payment response processor
        private readonly ServiceBusProcessor paymentResponseProcessor;

        private readonly IConfiguration _configuration;

        //payment response topic 
        private readonly string OrderUpdatePaymentResponseResultTopic;
      
        public AzureServiceBusConsumer(EmailRepository emailRepo, 
                                       IConfiguration configuration)
        {
            this._emailRepo = emailRepo;
            this._configuration = configuration;
           

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            //email sub - 2nd sub for orderupdatepaymentresponseresulttopic
            subscriptionEmail = _configuration.GetValue<string>("EmailSubscription");

            //payment response topic
            OrderUpdatePaymentResponseResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResponseResultTopic");
            

            //create client for the service bus
            var client = new ServiceBusClient(serviceBusConnectionString);
            //create process for the client which will pull the message from subscription of topic
            paymentResponseProcessor = client.CreateProcessor(OrderUpdatePaymentResponseResultTopic, subscriptionEmail);

        }

        //should be called and started when application is started
        public async Task StartProcessorAsync()
        {
            //start processing for getting payment response message
            paymentResponseProcessor.ProcessMessageAsync += OnOrderPaymentUpdateResponseMessageReceived;
            paymentResponseProcessor.ProcessErrorAsync += ErrorHandler;
            await paymentResponseProcessor.StartProcessingAsync();
        }

        //method will be called only when we stop the application
        public async Task StopProcessorAsync()
        {
            await paymentResponseProcessor.StopProcessingAsync();
            await paymentResponseProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args) 
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnOrderPaymentUpdateResponseMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentResponseMessage paymentResponseMessage = JsonConvert.DeserializeObject<PaymentResponseMessage>(body);

            try
            {
                await _emailRepo.SendAndLogEmail(paymentResponseMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
                //log the message
            }

        }
    }
}
