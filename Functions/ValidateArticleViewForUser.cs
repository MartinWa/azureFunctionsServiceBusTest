using System.Linq;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Test.AzureServiceBus;
using Test.AzureServiceBus.Messages;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Test.Enums;

namespace Test.Functions
{
    public class ValidateArticleViewForUser
    {
        private readonly ILogger _log;
        private readonly IServiceBusProxy _serviceBus;

        public ValidateArticleViewForUser(ILogger logger, IServiceBusProxy serviceBus)
        {
            _log = logger;
            _serviceBus = serviceBus;
        }

        [FunctionName("ValidateArticleViewForUser")]
        public async Task RunValidateArticleViewForUser(
            [ServiceBusTrigger(KnownTopics.Statistics.ArticleViewed, KnownSubscriptions.Statistics.ValidateArticleViewForUser,
                Connection = KnownConnections.Subscriber)]Message sbMessage)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var message = sbMessage.BodyAs<ArticleViewed>();
                AmendedLogTrace($"Validating ArticleView for ContentId: {message.ContentId}, Culture {message.Culture}", message);

                // Code to do work
                var nextMessage = new ArticleViewValidated(message.CorrelationId, message.CausationId)
                {
                    FactId = message.FactId,
                    Location = message.Location,
                    UserGroupIds = message.UserRights.UserGroupIds,
                    UserId = message.UserRights.UserId,
                    Platform = message.Platform,
                    Timestamp = message.Timestamp,
                    UserRole = GetHighestRole(message.UserRights.Roles),
                    ContentId = message.ContentId,
                    PortalId = message.UserRights.PortalId,
                    StatisticStartDate = message.StatisticStartDate,
                    AuthorUserId = 0,
                    KnowledgeState = KnowledgeState.Published,
                    TranslationId = 0
                };
                await _serviceBus.Send(KnownTopics.Statistics.ArticleViewValidated, nextMessage);
                stopWatch.Stop();
                AmendedLogTrace($"Done ({stopWatch.Elapsed}) validating ArticleView for ContentId: {message.ContentId}, Culture {message.Culture}", message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to validate ArticleView");
                throw;
            }
        }

        private static ZeroRole GetHighestRole(IEnumerable<ZeroRole> roles)
        {
            return roles.OrderByDescending(x => (int)x).FirstOrDefault();
        }


        private void AmendedLogTrace(string logMessage, MessageBase message)
        {
            var amendedMessage = logMessage + $", MessageId {message.MessageId}, CorrelationId {message.CorrelationId}, CausationId {message.CausationId}";
            _log.LogTrace(amendedMessage);
        }
    }
}
