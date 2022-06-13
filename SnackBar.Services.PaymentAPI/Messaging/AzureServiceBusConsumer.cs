using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentProcessor;
using SnackBar.MessageBus;
using SnackBar.Services.PaymentAPI.Messages;
using System.Text;

namespace SnackBar.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly string subscriptionPayment;
        private readonly string serviceBusConnectionString;
        private readonly string orderPaymentProcessTopic;

        private readonly string OrderUpdatePaymentResponseResultTopic;

        private ServiceBusProcessor orderPaymentProcessor;

        private readonly IProcessPayment _processPayment;

        private readonly IConfiguration _configuration;

        private readonly IMessageBus _messageBus;




        public AzureServiceBusConsumer(IConfiguration configuration,
                                        IMessageBus messageBus,
                                        IProcessPayment processPayment)
        {
            this._configuration = configuration;
            this._messageBus = messageBus;
            this._processPayment = processPayment; 

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");
            subscriptionPayment = _configuration.GetValue<string>("OrderPaymentSubscription");
            OrderUpdatePaymentResponseResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResponseResultTopic");

            //create client for the service bus
            var client = new ServiceBusClient(serviceBusConnectionString);
            //create process for the client which will pull the message from subscription of topic
            orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessTopic, subscriptionPayment);

        }

        //should be called and started when application is started
        public async Task StartProcessorAsync()
        {
            orderPaymentProcessor.ProcessMessageAsync += OnMessageReceivedProcessPayments;
            orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await orderPaymentProcessor.StartProcessingAsync();
        }

        //method will be called only when we stop the application
        public async Task StopProcessorAsync()
        {
            await orderPaymentProcessor.StopProcessingAsync();
            await orderPaymentProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args) 
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnMessageReceivedProcessPayments(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var StatusResult = _processPayment.PaymentProcessor();

            PaymentResponseMessage paymentResponseMessage = new PaymentResponseMessage()
            {
                Status = StatusResult,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email,
            };
            try
            {
                await _messageBus.PublishMessage(paymentResponseMessage, OrderUpdatePaymentResponseResultTopic);
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
