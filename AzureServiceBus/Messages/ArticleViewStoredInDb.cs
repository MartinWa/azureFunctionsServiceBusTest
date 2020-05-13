using System;
using Test.AzureServiceBus.Ids;

namespace Test.AzureServiceBus.Messages
{
    public class ArticleViewStoredInDb : MessageBase
    {
        public int ContentId { get; set;}
        public int PortalId { get; set;}
        public DateTime StatisticStartDate { get; set; }

        public ArticleViewStoredInDb(MessageId messageId, CorrelationId correlationId, CausationId causationId) : base(messageId, correlationId, causationId) { }
    }
}