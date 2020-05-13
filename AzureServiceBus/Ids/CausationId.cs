using System;
using Newtonsoft.Json;

namespace Test.AzureServiceBus.Ids
{
    [JsonConverter(typeof(CausationIdConverter))]
    public readonly struct CausationId : IEquatable<CausationId>, IComparable<CausationId>
    {
        private readonly Guid _value;

        internal CausationId(Guid id)
        {
            _value = id;
        }

        public static CausationId Create(MessageId eventId) => new CausationId(eventId.Value);
        public static CausationId Empty => new CausationId(Guid.Empty);


        public override bool Equals(object obj)
        {
            return obj is CausationId id && Equals(id);
        }

        public bool Equals(CausationId other)
        {
            return _value.Equals(other._value);
        }

        public int CompareTo(CausationId other)
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

    public class CausationIdConverter : JsonConverter<CausationId>
    {
        public override void WriteJson(JsonWriter writer, CausationId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override CausationId ReadJson(JsonReader reader, Type objectType, CausationId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var s = reader?.Value?.ToString();
            return string.IsNullOrEmpty(s) ? CausationId.Empty : new CausationId(Guid.Parse(reader.Value.ToString()));
        }
    }
}