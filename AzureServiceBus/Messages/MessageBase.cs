using System;
using System.Text;
using Newtonsoft.Json;

namespace Test.AzureServiceBus.Messages
{
    public abstract class MessageBase
    {
        public Guid MessageId { get; set; }
        public Guid? CausationId { get; set; }
        public Guid? CorrelationId { get; set; }
        public string ContentType => "application/json";

        protected MessageBase() : this(Guid.NewGuid(), Guid.NewGuid(), null) { }

        protected MessageBase(Guid messageId, Guid? correlationId, Guid? causationId)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public byte[] AsJson()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}