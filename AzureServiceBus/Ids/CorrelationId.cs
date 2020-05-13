using System;
using Newtonsoft.Json;

namespace Test.AzureServiceBus.Ids
{
    [JsonConverter(typeof(CorrelationIdConverter))]
    public readonly struct CorrelationId : IEquatable<CorrelationId>, IComparable<CorrelationId>
    {
        private readonly Guid _value;

        internal CorrelationId(Guid id)
        {
            _value = id;
        }

        public static CorrelationId NewId => new CorrelationId(Guid.NewGuid());

        public override bool Equals(object obj)
        {
            return obj is CorrelationId id && Equals(id);
        }

        public bool Equals(CorrelationId other)
        {
            return _value.Equals(other._value);
        }

        public int CompareTo(CorrelationId other)
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

    public class CorrelationIdConverter : JsonConverter<CorrelationId>
    {
        public override void WriteJson(JsonWriter writer, CorrelationId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override CorrelationId ReadJson(JsonReader reader, Type objectType, CorrelationId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new CorrelationId(Guid.Parse(reader?.Value?.ToString() ?? throw new ArgumentException("CorrelationId.Value is null and cant be deserialized")));
        }
    }
}