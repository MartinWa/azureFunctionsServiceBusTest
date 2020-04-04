using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Test.Settings;
using Microsoft.Azure.ServiceBus.Management;
using Test.AzureServiceBus;

namespace Test.Functions
{
    public class StatusMessages
    {
        private readonly ILogger _log;
        private readonly ServiceBusSettings _serviceBusSettings;

        public StatusMessages(ILogger logger, IOptionsMonitor<ServiceBusSettings> serviceBusSettings)
        {
            _log = logger;
            _serviceBusSettings = serviceBusSettings.CurrentValue;
        }

        [FunctionName("StatusMessages")]
        public async Task<IActionResult> RunStatusMessages([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status/messages")]HttpRequest req)
        {
            try
            {
                var result = new List<TopicStatus>();
                var manager = new ManagementClient(_serviceBusSettings.ConnectionStringManager);
                result.Add(await GetTopicStatus(manager, KnownTopics.Statistics.ArticleViewed, new[] { KnownSubscriptions.Statistics.ValidateArticleViewForUser }));
                result.Add(await GetTopicStatus(manager, KnownTopics.Statistics.ArticleViewValidated, new[] { KnownSubscriptions.Statistics.WriteArticleViewToStatisticsDb }));
                result.Add(await GetTopicStatus(manager, KnownTopics.Statistics.ArticleViewStoredInDb, new[] { KnownSubscriptions.Statistics.UpdateSearch, KnownSubscriptions.Statistics.UpdateCache }));
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"{ex.Message}");
            }
            return new JsonResult(new { value = "Error reading service bus messages" });
        }

        private async Task<TopicStatus> GetTopicStatus(ManagementClient manager, string topic, IEnumerable<string> subscriptions)
        {
            var topicInfo = await manager.GetTopicRuntimeInfoAsync(topic);
            var subscriptionInfoTasks = subscriptions.Select(subscription => GetSubscriptionStatus(manager, topic, subscription));
            var subscriptionInfo = await Task.WhenAll(subscriptionInfoTasks);
            return new TopicStatus
            {
                Topic = topicInfo.Path,
                AccessedAt = topicInfo.AccessedAt,
                ActiveMessageCount = topicInfo.MessageCountDetails.ActiveMessageCount,
                ScheduledMessageCount = topicInfo.MessageCountDetails.ScheduledMessageCount,
                DeadLetterMessageCount = topicInfo.MessageCountDetails.DeadLetterMessageCount,
                SubscriptionCount = topicInfo.SubscriptionCount,
                SubscriptionInfo = subscriptionInfo
            };
        }

        private async Task<SubscriptionStatus> GetSubscriptionStatus(ManagementClient manager, string topic, string subscription)
        {
            var subscriptionInfo = await manager.GetSubscriptionRuntimeInfoAsync(topic, subscription);
            return new SubscriptionStatus
            {
                Subscription = subscriptionInfo.SubscriptionName,
                AccessedAt = subscriptionInfo.AccessedAt,
                TotalMessageCount = subscriptionInfo.MessageCount,
                ActiveMessageCount = subscriptionInfo.MessageCountDetails.ActiveMessageCount,
                ScheduledMessageCount = subscriptionInfo.MessageCountDetails.ScheduledMessageCount,
                DeadLetterMessageCount = subscriptionInfo.MessageCountDetails.DeadLetterMessageCount
            };
        }
    }

    internal class TopicStatus
    {
        public string Topic { get; internal set; }
        public DateTime AccessedAt { get; internal set; }
        public long ActiveMessageCount { get; internal set; }
        public long ScheduledMessageCount { get; internal set; }
        public long DeadLetterMessageCount { get; internal set; }
        public int SubscriptionCount { get; internal set; }
        public IEnumerable<SubscriptionStatus> SubscriptionInfo { get; internal set; }
    }

    internal class SubscriptionStatus
    {
        public string Subscription { get; internal set; }
        public DateTime AccessedAt { get; internal set; }
        public long TotalMessageCount { get; internal set; }
        public long ActiveMessageCount { get; internal set; }
        public long ScheduledMessageCount { get; internal set; }
        public long DeadLetterMessageCount { get; internal set; }
    }
}