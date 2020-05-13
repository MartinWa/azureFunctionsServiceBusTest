using Test.AzureServiceBus.Ids;

namespace Test.AzureServiceBus.Messages
{
    public abstract class MessageBase
    {
        /// <summary>
        ///  MessageId is a unique id for each message
        /// </summary>
        public MessageId MessageId { get; set; }
        /// <summary>
        /// CausationId is the id of the message that caused this message to be created.
        /// The CausationId can be used to see the flow (or breadcrumbs, so to speak) of a specific message.
        /// If [ArticleViewed] in turn generates a [ArticleViewStoredInDb] then [ArticleViewStoredIdDb]::CausationId should be set to [ArticleViewed].MessageId.
        /// If a parent message generates a group of child messages they will all have the same CausationId.
        /// </summary>
        public CausationId CausationId { get; set; }
        /// <summary>
        /// CorrelationId is an id to tie together a group of messages, for example when an batch update is made, then all messages in the same batch should have the same CorrelationId.
        /// E.g. a POST to statistics/view/bulk adds a batch of ArticleViews. The messages created as a result of this HttpRequest should all have the same CorrelationId.
        /// </summary>
        public CorrelationId CorrelationId { get; set; }
        public string ContentType => "application/json";

        protected MessageBase(MessageId messageId, CorrelationId correlationId, CausationId causationId)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            CausationId = causationId;
        }
    }
}