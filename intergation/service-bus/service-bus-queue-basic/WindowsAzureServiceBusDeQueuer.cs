using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace service_bus_queue_basic
{
    internal class WindowsAzureServiceBusDeQueuer
    {
        internal static async Task DeQueueAll()
        {
            var receiverFactory = MessagingFactory.CreateFromConnectionString(Configurations.SBConnectionString);

            var receiver = await receiverFactory.CreateMessageReceiverAsync(Configurations.QueueName, ReceiveMode.PeekLock);

            Console.WriteLine("Receiving message from Queue...");
            while (true)
            {
                try
                {
                    //receive messages from Queue
                    var message = await receiver.ReceiveAsync(TimeSpan.FromSeconds(5));
                    if (message != null)
                    {
                        if (message.Label != null &&
                            message.ContentType != null &&
                            message.Label.Equals("TestMessage", StringComparison.InvariantCultureIgnoreCase) &&
                            message.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var body = message.GetBody<Stream>();

                            dynamic msgObj = JsonConvert.DeserializeObject(new StreamReader(body, true).ReadToEnd());

                            lock (Console.Out)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(
                                    $@"Message received:
                                        MessageId = {message.MessageId},
                                        SequenceNumber = {message.SequenceNumber}, 
                                        EnqueuedTimeUtc = {message.EnqueuedTimeUtc},
                                        ExpiresAtUtc = {message.ExpiresAtUtc},
                                        ContentType = { message.ContentType},
                                        Size = {message.Size},
                                        Content: [ id = {msgObj.id}, text = {msgObj.text} ]");
                                Console.ResetColor();
                            }
                            await message.CompleteAsync();
                        }
                        else
                        {
                            await message.DeadLetterAsync("ProcessingError", "Don't know what to do with this message");
                        }
                    }
                    else
                    {
                        //no more messages in the queue
                        break;
                    }
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }
    }
}
