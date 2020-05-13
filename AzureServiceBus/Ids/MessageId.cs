using System;
using Newtonsoft.Json;

namespace Test.AzureServiceBus.Ids
{
    [JsonConverter(typeof(EventIdConverter))]
    public readonly struct MessageId : IEquatable<MessageId>, IComparable<MessageId>
    {
        private readonly Guid _value;

        internal MessageId(Guid id)
        {
            _value = id;
        }

        internal Guid Value => _value;

        public static MessageId NewId => new MessageId(Guid.NewGuid());
        public static MessageId Create(string id) => new MessageId(Guid.Parse(id));
       
        public override bool Equals(object obj)
        {
            return obj is MessageId id && Equals(id);
        }

        public bool Equals(MessageId other)
        {
            return _value.Equals(other._value);
        }

        public int CompareTo(MessageId other)
        {
            return _value.CompareTo(other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString("N");
        }
    }

    public class EventIdConverter : JsonConverter<MessageId>
    {
        public override void WriteJson(JsonWriter writer, MessageId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override MessageId ReadJson(JsonReader reader, Type objectType, MessageId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new MessageId(Guid.Parse(reader?.Value?.ToString() ?? throw new ArgumentException("EventId.Value is null and cant be deserialized")));
        }
    }
}