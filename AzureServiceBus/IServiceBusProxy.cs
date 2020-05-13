using System.Collections.Generic;
using System.Threading.Tasks;
using Test.AzureServiceBus.Messages;

namespace Test.AzureServiceBus
{
    public interface IServiceBusProxy
    {
        Task Send(string topic, IEnumerable<MessageBase> messages, int batchSize = Constants.BatchSizeServiceBus);
        Task Send(string topic, MessageBase evnt);
    }
}
