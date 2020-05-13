using System;
using Test.AzureServiceBus.Ids;
using Test.Dto;

namespace Test.AzureServiceBus.Messages
{
    public class ArticleViewed : MessageBase
    {
        public Guid FactId { get; set; }
        public int ContentId { get; set; }
        public string Culture { get; set; }
        public Location Location { get; set; }
        public UserRightsDto UserRights { get; set; }
        public DateTime Timestamp { get; set; }
        public int Platform { get; set; }
        public DateTime StatisticStartDate { get; set; }

        public ArticleViewed(MessageId messageId, CorrelationId correlationId) : base(messageId, correlationId, CausationId.Empty) { }
    }
}