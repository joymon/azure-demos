using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace service_bus_queue_basic
{
    internal class AzureMessagingServiceBusDequeuer
    {
        internal static async Task DeQueueAll()
        {
            var client = new ServiceBusClient(Configurations.SBConnectionString);
            var opts = new ServiceBusReceiverOptions
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            };
            var receiver = client.CreateReceiver(Configurations.QueueName, opts);
            Console.WriteLine($"{nameof(AzureMessagingServiceBusDequeuer)} Started receiving ");
            while (true)
            {
                try
                {
                    IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(10, TimeSpan.FromSeconds(5));

                    if (messages.Any())
                    {
                        foreach (ServiceBusReceivedMessage message in messages)
                        {
                            IReadOnlyDictionary<string, object> myApplicationProperties = message.ApplicationProperties;

                            var body = message.Body.ToStream();
                            dynamic msgObj = JsonConvert.DeserializeObject(new StreamReader(body, true).ReadToEnd());

                            Console.ForegroundColor = ConsoleColor.Cyan;

                            Console.WriteLine($@"MessageId = {message.MessageId},
                                                Id={message.MessageId},
                                                SequenceNumber = {message.SequenceNumber}, 
                                                EnqueuedTimeUtc = {message.EnqueuedTime},
                                                ExpiresAtUtc = {message.TimeToLive},
                                                ContentType = { message.ContentType},
                                                Content:[ id = {msgObj.id}, Text={msgObj.text}]");
                            Console.ResetColor();
                            await receiver.CompleteMessageAsync(message);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            await receiver.CloseAsync();
            Console.WriteLine($"{nameof(AzureMessagingServiceBusDequeuer)} Completed receiving ");
        }
    }
}