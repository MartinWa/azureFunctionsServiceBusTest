using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Test.AzureServiceBus.Messages;
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

        public Task Send(string topic, MessageBase evnt) => Send(topic, new[] {evnt});
        public Task Send(string topic, IEnumerable<MessageBase> messages, int batchSize = Constants.BatchSizeServiceBus) => SendCore(topic, messages, batchSize);

        private async Task SendCore(string topic, IEnumerable<MessageBase> messages, int batchSize)
        {
            var enumerated = messages.ToList();
            if (!enumerated.Any())
                return;
                
            var client = _senders.GetOrAdd(topic, t =>
                new Lazy<MessageSender>(() =>
                    new MessageSender(_serviceBusSettings.ConnectionStringSender, t)));

            foreach (var batch in enumerated.Batch(batchSize)) // ServiceBus has a max of 256KB (standard) and 1MB (premium). 
            {
                var batchPartitionKey = Guid.NewGuid().ToString("N");
                await client.Value.SendAsync(batch.Select(x => new Message(ToByteArray(x))
                {
                    ContentType = x.ContentType,
                    MessageId = x.MessageId.ToString(),
                    PartitionKey = batchPartitionKey
                }).ToList());
            }
        }

        private byte[] ToByteArray(MessageBase msg) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
    }
}
