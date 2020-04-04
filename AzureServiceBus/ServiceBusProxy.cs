using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.AzureServiceBus.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Options;
using Test.Settings;

namespace Test.AzureServiceBus
{
    public class ServiceBusProxy : IServiceBusProxy
    {
        private readonly ServiceBusSettings _serviceBusSettings;
        private readonly ConcurrentDictionary<string, Lazy<MessageSender>> _senders = new ConcurrentDictionary<string, Lazy<MessageSender>>();

        public ServiceBusProxy(IOptionsMonitor<ServiceBusSettings> serviceBusSettings)
        {
            _serviceBusSettings = serviceBusSettings.CurrentValue;
        }

        public Task Send(string topic, IEnumerable<MessageBase> messages) => SendCore(topic, messages);

        public Task Send(string topic, params MessageBase[] messages) => SendCore(topic, messages);

        private async Task SendCore(string topic, IEnumerable<MessageBase> messages)
        {
            var enumerated = messages as MessageBase[] ?? messages.ToArray();
            if (!enumerated.Any())
                return;

            var client = _senders.GetOrAdd(topic, t =>
                new Lazy<MessageSender>(() =>
                    new MessageSender(_serviceBusSettings.ConnectionStringSender, t)));

            foreach (var batch in enumerated.Batch(Constants.BatchSizeServiceBus)) // ServiceBus has a max of 256KB (standard) and 1MB (premium). 
            {
                var batchPartitionKey = Guid.NewGuid().ToString("N");
                await client.Value.SendAsync(batch.Select(x => new Message(x.AsJson())
                {
                    ContentType = x.ContentType,
                    MessageId = x.MessageId.ToString("N"),
                    PartitionKey = batchPartitionKey
                }).ToList());
            }
        }
    }
}
