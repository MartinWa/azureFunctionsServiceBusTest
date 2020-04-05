using System.Linq;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Test.AzureServiceBus;
using Test.AzureServiceBus.Messages;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Test.Enums;

namespace Test.Functions
{
    public class WriteArticleViewToStatisticsDb
    {
        private readonly ILogger _log;
        private readonly IServiceBusProxy _serviceBus;

        public WriteArticleViewToStatisticsDb(ILogger logger, IServiceBusProxy serviceBus)
        {
            _log = logger;
            _serviceBus = serviceBus;
        }

        [FunctionName("WriteArticleViewToStatisticsDb")]
        public async Task RunWriteArticleViewToStatisticsDb([ServiceBusTrigger(KnownTopics.Statistics.ArticleViewValidated, KnownSubscriptions.Statistics.WriteArticleViewToStatisticsDb, Connection = KnownConnections.Subscriber)]Message[] sbMessages)
        {
            try
            {
                const int batchSize = Constants.BatchSizeStatisticsDb;
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                _log.LogInformation($"ReceiveBatch updating statistics database, count: {sbMessages.Length}");
                _log.LogInformation($"Making {Math.Ceiling((double)sbMessages.Length / batchSize)} batches");

                foreach (var sbBatch in sbMessages.Batch(Constants.BatchSizeStatisticsDb))
                {
                    var batch = sbBatch.Select(m => m.BodyAs<ArticleViewValidated>()).ToList();
                    var dbModels = CreateDbModelBatches(batch).ToList();
                    await InsertViews(dbModels.Select(x => x.messageModel));
                    await InsertViewUserGroups(dbModels.SelectMany(x => x.userGroupModels));
                    var nextMessages = BuildNextMessages(batch);
                    await _serviceBus.Send(KnownTopics.Statistics.ArticleViewStoredInDb, nextMessages);
                }

                stopWatch.Stop();
                _log.LogInformation($"Done ({stopWatch.Elapsed}) batch updating statistics database, sent: {sbMessages.Length} messages out");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to write ArticleView to statistics db.");
                throw;
            }
        }

        private static IEnumerable<ArticleViewStoredInDb> BuildNextMessages(IEnumerable<ArticleViewValidated> messages) =>
            messages.Select(msg => new ArticleViewStoredInDb(msg.CorrelationId, msg.CausationId)
            {
                ContentId = msg.ContentId,
                PortalId = msg.PortalId,
                StatisticStartDate = msg.StatisticStartDate
            });

        private async Task InsertViews(IEnumerable<ViewDbModel> viewModels)
        {
            // Code to write to db
            await Task.CompletedTask;

        }

        private async Task InsertViewUserGroups(IEnumerable<UserGroupDbModel> userGroupModels)
        {
            // Code to write to db
            await Task.CompletedTask;

        }

        private static IEnumerable<(ViewDbModel messageModel, IEnumerable<UserGroupDbModel> userGroupModels)> CreateDbModelBatches(IEnumerable<ArticleViewValidated> messages) =>
            messages.Select(message =>
            {
                var messageModel = new ViewDbModel
                {
                    FactId = message.MessageId,
                    ContentId = message.ContentId,
                    Date = message.Timestamp.ToString(),
                    PortalId = message.PortalId,
                    RoleId = (int)message.UserRole,
                    UserId = message.UserId,
                    UtcDate = message.Timestamp,
                    City = message.Location?.City,
                    Province = message.Location?.Province,
                    Country = message.Location?.Country,
                    Continent = message.Location?.Continent,
                    KnowledgeStateId = message.KnowledgeState,
                    Time = new TimeSpan(message.Timestamp.Hour, message.Timestamp.Minute, 0),
                    AuthorUserId = message.AuthorUserId,
                    PlatformId = message.Platform
                };
                var userGroupModels = CreateUserGroupModels(message.MessageId, message.UserGroupIds);
                return (messageModel, userGroupModels);
            });

        private static IEnumerable<UserGroupDbModel> CreateUserGroupModels(Guid factId, IEnumerable<int> userGroupIds)
        {
            if (userGroupIds == null)
            {
                return Enumerable.Empty<UserGroupDbModel>();
            }
            return userGroupIds.Select(ugid => new UserGroupDbModel
            {
                FactId = factId,
                GroupId = ugid
            });
        }
    }

    public class ViewDbModel
    {
        public Guid FactId { get; set; }
        public int ContentId { get; set; }
        public int TranslationId { get; set; }
        public string Date { get; set; }
        public int PortalId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public DateTime UtcDate { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public KnowledgeState KnowledgeStateId { get; set; }
        public TimeSpan Time { get; set; }
        public int AuthorUserId { get; set; }
        public int PlatformId { get; set; }
    }

    public class UserGroupDbModel
    {
        public Guid FactId { get; set; }
        public int GroupId { get; set; }
    }

}
