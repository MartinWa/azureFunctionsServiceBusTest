using System;

namespace Test.AzureServiceBus.Messages
{
    public class ArticleViewStoredInDb : MessageBase
    {
        public int ContentId { get; set;}
        public int PortalId { get; set;}
        public DateTime StatisticStartDate { get; set; }

        private ArticleViewStoredInDb() : base() { } 

        public ArticleViewStoredInDb(Guid? correlationId, Guid? causationId) : base(Guid.NewGuid(), correlationId, causationId)
        { }
    }
}