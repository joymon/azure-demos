using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace service_bus_queue_basic
{
    public class MicrosoftAzureServiceBusDequeuer : IServiceBusDequeuer
    {
        async Task IServiceBusDequeuer.DeQueueAll()
        {
            var messageReceiver = new MessageReceiver(Configurations.SBConnectionString, 
                                                    Configurations.QueueName, 
                                                    ReceiveMode.PeekLock);
            Console.WriteLine($"{nameof(MicrosoftAzureServiceBusDequeuer)} Started receiving ");
            while (true)
            {
                try
                {
                    Message message = await messageReceiver.ReceiveAsync(TimeSpan.FromSeconds(5));
                    if (message == null)
                    {
                        break;
                    }
                    else
                    {
                        var body = Encoding.UTF8.GetString(message.Body);
                        dynamic msgObj = JsonConvert.DeserializeObject(new StringReader(body).ReadToEnd());

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($@"MessageId={message.MessageId},
                                        SequenceNumber:{message.SystemProperties.SequenceNumber},
                                        Content:[id={msgObj.id}, Text={msgObj.text}]");
                        Console.ResetColor();
                        
                        await messageReceiver.CompleteAsync(message.SystemProperties.LockToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            Console.WriteLine($"{nameof(MicrosoftAzureServiceBusDequeuer)} Completed receiving ");
        }
    }
}