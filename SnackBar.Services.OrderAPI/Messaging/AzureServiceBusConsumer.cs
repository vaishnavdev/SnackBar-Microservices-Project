using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SnackBar.MessageBus;
using SnackBar.Services.OrderAPI.Messages;
using SnackBar.Services.OrderAPI.Models;
using SnackBar.Services.OrderAPI.Repository;
using System.Text;

namespace SnackBar.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly OrderRepository _orderRepo;
        private readonly string subscriptionCheckout;
        private readonly string checkoutMessageTopic;
        private readonly string serviceBusConnectionString;

        private ServiceBusProcessor checkOutProcessor;

        //payment response processor
        private readonly ServiceBusProcessor paymentResponseProcessor;

        private readonly IConfiguration _configuration;

        private readonly IMessageBus _messageBus;

        private readonly string orderPaymentProcessTopic;


        //payment response topic and sub
        private readonly string OrderUpdatePaymentResponseResultTopic;
        private readonly string PaymentResponseResultSubscription;


        public AzureServiceBusConsumer(OrderRepository orderRepo, 
                                       IConfiguration configuration,
                                        IMessageBus messageBus)
        {
            this._orderRepo = orderRepo;
            this._configuration = configuration;
            this._messageBus = messageBus;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            subscriptionCheckout = _configuration.GetValue<string>("subscriptionCheckout");

            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");

            //payment response topic and sub
            OrderUpdatePaymentResponseResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResponseResultTopic");
            PaymentResponseResultSubscription = _configuration.GetValue<string>("PaymentResponseResultSubscription");

            //create client for the service bus
            var client = new ServiceBusClient(serviceBusConnectionString);
            //create process for the client which will pull the message from subscription of topic
            checkOutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckout);
            //payment processor 
            paymentResponseProcessor = client.CreateProcessor(OrderUpdatePaymentResponseResultTopic, PaymentResponseResultSubscription);

        }

        //should be called and started when application is started
        public async Task StartProcessorAsync()
        {
            //start processing for checkout message
            checkOutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
            checkOutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkOutProcessor.StartProcessingAsync();

            //start processing for getting payment response message
            paymentResponseProcessor.ProcessMessageAsync += OnOrderPaymentUpdateResponseMessageReceived;
            paymentResponseProcessor.ProcessErrorAsync += ErrorHandler;
            await paymentResponseProcessor.StartProcessingAsync();
        }

        //method will be called only when we stop the application
        public async Task StopProcessorAsync()
        {
            await checkOutProcessor.StopProcessingAsync();
            await checkOutProcessor.DisposeAsync();

            await paymentResponseProcessor.StopProcessingAsync();
            await paymentResponseProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args) 
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }


        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkOutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkOutHeaderDto.UserId,
                FirstName = checkOutHeaderDto.FirstName,
                LastName = checkOutHeaderDto.LastName,

                OrderDetails = new List<OrderDetails>(),

                CardNumber = checkOutHeaderDto.CardNumber,
                // CartTotalItems = checkOutHeaderDto.CartTotalItems,
                CouponCode = checkOutHeaderDto.CouponCode,
                CVV = checkOutHeaderDto.CVV,
                DiscountTotal = checkOutHeaderDto.DiscountTotal,
                Email = checkOutHeaderDto.Email,
                ExpiryMonthYear = checkOutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkOutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkOutHeaderDto.Phone,
                PickUpDateTime = checkOutHeaderDto.PickUpDateTime
            };

            foreach(var detail in checkOutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detail.ProductId,
                    ProductName = detail.Product.Name,
                    ProductPrice = detail.Product.Price,
                    Count = detail.Count
                };
                orderHeader.CartTotalItems+= detail.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }
            await _orderRepo.SaveOrder(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new PaymentRequestMessage()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email,
            };
            try
            {
                await _messageBus.PublishMessage(paymentRequestMessage, orderPaymentProcessTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
                //log the message
            }
        }
        private async Task OnOrderPaymentUpdateResponseMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentResponseMessage paymentResponseResultMessage = JsonConvert.DeserializeObject<PaymentResponseMessage>(body);

            await _orderRepo.UpdateOrderPaymentStatus(paymentResponseResultMessage.OrderId, paymentResponseResultMessage.Status);
            await args.CompleteMessageAsync(args.Message);

        }
    }
}
