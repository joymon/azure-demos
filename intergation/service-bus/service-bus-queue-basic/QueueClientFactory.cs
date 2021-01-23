using Microsoft.Azure.ServiceBus;

namespace service_bus_queue_basic
{
    internal class QueueClientFactory
    {
        internal static QueueClient GetQueueClient()
        {
            return new QueueClient(Configurations.SBConnectionString, Configurations.QueueName);
        }
    }
}
