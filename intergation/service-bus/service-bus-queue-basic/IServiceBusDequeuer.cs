using System.Threading.Tasks;

namespace service_bus_queue_basic
{
    public interface IServiceBusDequeuer
    {
        Task DeQueueAll();
    }
}