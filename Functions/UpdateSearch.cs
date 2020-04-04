using System;
using System.Linq;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Test.AzureServiceBus;
using Test.AzureServiceBus.Messages;
using System.Diagnostics;

namespace Test.Functions
{
    public class UpdateSearch
    {
        private readonly ILogger _log;

        public UpdateSearch(ILogger log)
        {
            _log = log;
        }

        [FunctionName("UpdateSearch")]
        public async Task RunUpdateSearch([ServiceBusTrigger(KnownTopics.Statistics.ArticleViewStoredInDb, KnownSubscriptions.Statistics.UpdateSearch, Connection = KnownConnections.Subscriber)]Message[] sbMessages)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var messages = sbMessages.Select(m => m.BodyAs<ArticleViewStoredInDb>()).ToList();
                _log.LogTrace($"ReceiveBatch updating search, count: {messages.Count}.");
                var groupedByPortalId = messages.GroupBy(x => x.PortalId).ToList();
                _log.LogTrace($"Making {groupedByPortalId.Count} groups on portalId");
                foreach (var group in groupedByPortalId)
                {
                    var batches = group.Batch(Constants.BatchSizeAzureSearchFilter);
                    _log.LogTrace($"Making {batches.Count()} batches");
                    foreach (var batch in batches)
                    {
                        // Code to do work
                    }
                }
                stopWatch.Stop();
                _log.LogTrace($"Done ({stopWatch.Elapsed}) batch updating search");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Failed to update ArticleView in AzureSearch");
                throw;
            }
        }
    }
}
