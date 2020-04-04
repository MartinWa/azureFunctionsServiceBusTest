using System;
using System.Collections.Generic;
using Test.Enums;

namespace Test.AzureServiceBus.Messages
{
    public class ArticleViewValidated : MessageBase
    {
        public Guid FactId { get; set; }
        public int ContentId { get; set; }
        public Location Location { get; set; }
        public int PortalId { get; set; }
        public int TranslationId { get; set; }
        public ZeroRole UserRole { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public KnowledgeState KnowledgeState { get; set; }
        public int AuthorUserId { get; set; }
        public int Platform { get; set; }
        public IEnumerable<int> UserGroupIds { get; set; }
        public DateTime StatisticStartDate { get; set; }

        private ArticleViewValidated() : base() { }

        public ArticleViewValidated(Guid? correlationId, Guid? causationId) : base(Guid.NewGuid(), correlationId, causationId) { }
    }
}