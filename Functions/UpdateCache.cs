using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Test.AzureServiceBus;
using Test.AzureServiceBus.Messages;

namespace Test.Functions
{
    public class UpdateCache
    {
        private readonly ILogger _log;

        public UpdateCache(ILogger log)
        {
            _log = log;
        }

        [FunctionName("UpdateCache")]
        public async Task RunUpdateCache([ServiceBusTrigger(KnownTopics.Statistics.ArticleViewStoredInDb, KnownSubscriptions.Statistics.UpdateCache, Connection = KnownConnections.Subscriber)]Message[] sbMessages)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var messages = sbMessages.Select(m => m.BodyAs<ArticleViewStoredInDb>()).ToList();
                _log.LogTrace($"ReceiveBatch updating cache, count: {messages.Count}.");
                // Code to do work 
                await Task.CompletedTask;
                stopWatch.Stop();
                _log.LogTrace($"Done ({stopWatch.Elapsed}) batch updating cache");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to update ArticleView in cache.");
                throw;
            }
        }
    }
}
