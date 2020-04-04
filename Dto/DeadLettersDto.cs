using System.Collections.Generic;

namespace Test.Dto
{
    public class DeadLettersDto
    {
        public DeadLettersDto(string topic, string subscription)
        {
            Topic = topic;
            Subscription = subscription;
        }

        public string Topic { get; }
        public string Subscription { get; }
        public List<MessageDto> Messages { get; } = new List<MessageDto>();
    }
}