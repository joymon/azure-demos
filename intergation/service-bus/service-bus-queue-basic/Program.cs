using CSharp.Console.Menu;
using EasyConsole;
using Microsoft.Azure.ServiceBus;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace service_bus_queue_basic
{
    class Program
    {
        static void Main(string[] args)
        {
            CSharp.Console.Menu.Menu menu = new CSharp.Console.Menu.Menu("ServiceBus", "SB interaction");
            //menu.Start();
            Execute().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task Execute()
        {
            try
            {
                var easyConsoleMenu = new EasyConsole.Menu()
                                .Add("Queue Messages", () => QueueMessages())
                                .Add("Dequeue Messages(Microsoft.Azure.ServiceBus nuget)", async () => await DeQueueMessages( ServiceBusNugetLibraries.MicrosoftAzureServiceBus))
                                .Add("Dequeue Messages(Azure.Messaging.ServiceBus nuget)", async () => await DeQueueMessages(ServiceBusNugetLibraries.AzureMessagingServiceBus));

                easyConsoleMenu.Display();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task DeQueueMessages(ServiceBusNugetLibraries libraryType)
        {
            Console.WriteLine("Dequue");
            //await WindowsAzureServiceBusDeQueuer.DeQueueAll();
            IServiceBusDequeuer dequeuer = ServiceBusDequeuerFactory.Get(libraryType);
            await dequeuer.DeQueueAll();
        }

        static void QueueMessages()
        {
            int messageCount = Input.ReadInt("Enter the Number of messages", 0, 1000);
            Enumerable.Range(0, messageCount).Select(n => MicrosoftAzureServiceBusQueuer.Queue(n)).ToList();
            Console.WriteLine($"Queued {messageCount} messages");
        }
    }
    enum ServiceBusNugetLibraries
    {
        MicrosoftAzureServiceBus,
        AzureMessagingServiceBus
    }
    class ServiceBusDequeuerFactory
    {
        public static IServiceBusDequeuer Get(ServiceBusNugetLibraries libraryType)
        {
            if (libraryType == ServiceBusNugetLibraries.AzureMessagingServiceBus)
                return new AzureMessagingServiceBusDequeuer();
            else
                return new MicrosoftAzureServiceBusDequeuer();
        }
    }
}
