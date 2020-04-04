using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Options;
using Test.Settings;
using Test.Dto;

namespace Test.Functions
{
    public class PurgeDeadLetterMessages
    {
        private readonly ILogger _log;
        private readonly ServiceBusSettings _serviceBusSettings;

        public PurgeDeadLetterMessages(ILogger logger, IOptionsMonitor<ServiceBusSettings> serviceBusSettings)
        {
            _log = logger;
            _serviceBusSettings = serviceBusSettings.CurrentValue;
        }

        [FunctionName("PurgeDeadLetterMessages")]
        public async Task<IActionResult> RunPurgeDeadLetterMessages([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{topic}/{subscription}/purge")]HttpRequest req, string topic, string subscription)
        {
            var deadLetterPath = $"{topic}/Subscriptions/{subscription}/$deadletterqueue";
            try
            {
                var result = new DeadLettersDto(topic, subscription);
                var receiver = new MessageReceiver(_serviceBusSettings.ConnectionStringManager, deadLetterPath, ReceiveMode.ReceiveAndDelete);
                while (true)
                {
                    var messages = await receiver.ReceiveAsync(1000, TimeSpan.FromSeconds(5));
                    if (messages == null || messages.Count == 0) break;
                    result.Messages.AddRange(
                        messages.Select(m => 
                            new MessageDto
                            {
                                DeliveryCount = m.SystemProperties.DeliveryCount, 
                                BodyString = System.Text.Encoding.UTF8.GetString(m.Body)
                            }));
                    _log.LogInformation($"Purged {result.Messages.Count} messages from {deadLetterPath}");
                }
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"{ex.Message}");
            }
            return new JsonResult(new { value = $"Error reading service bus messages from {deadLetterPath}" });
        }
    }
}
