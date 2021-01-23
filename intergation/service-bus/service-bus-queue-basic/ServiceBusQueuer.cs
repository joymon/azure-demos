using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace service_bus_queue_basic
{
    internal class MicrosoftAzureServiceBusQueuer
    {
        internal static object Queue(int n)
        {
            string messageString = $"{{'id':'{n}','text':'This is {n}th message'}}";
            Message msg = new Message(Encoding.UTF8.GetBytes(messageString));
            msg.ContentType = "application/json";
            msg.Label = "TestMessage";
            QueueClient queueClient = QueueClientFactory.GetQueueClient();

            queueClient.SendAsync(msg).Wait();
            Console.WriteLine($"Queued Message No :{n}");
            return true;
        }
    }
}
