//using Microsoft.Azure.ServiceBus;
//using Microsoft.Azure.ServiceBus.Core;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBar.MessageBus;
    public class AzureServiceBusMessageBus : IMessageBus
    {
        private string AzureBusconnectionString = "Endpoint=sb://snackbarrestaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RnNXUZXeofk89YeWWJb6BuhwDrZ0eECGxLjsWzYPm4U=";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            
            //ISenderClient senderClient = new TopicClient(AzureBusconnectionString, topicName);
            ServiceBusClient client = new ServiceBusClient(AzureBusconnectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var JsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage FinalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(FinalMessage);
            await client.DisposeAsync();

            //await senderClient.SendAsync(FinalMessage);
            //await senderClient.CloseAsync();
        }
    }
