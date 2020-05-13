using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Test.AzureServiceBus;
using Test.AzureServiceBus.Ids;
using Test.AzureServiceBus.Messages;
using Test.Dto;
using Test.Enums;

namespace Test.Functions
{
    public class PostViews
    {
        private readonly ILogger _log;
        private readonly IServiceBusProxy _serviceBus;

        public PostViews(ILogger logger, IServiceBusProxy serviceBus)
        {
            _log = logger;
            _serviceBus = serviceBus;
        }

        [FunctionName("PostViews")]
        public async Task<IActionResult> RunPostViews([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "postviews/{count}")]HttpRequest req, int count)
        {
            if (count < 1 || count > 100000)
            {
                return new BadRequestResult();
            }
            var messages = new List<ArticleViewed>();
            for (int i = 0; i < count; i++)
            {
                messages.Add(new ArticleViewed(MessageId.NewId, CorrelationId.NewId)
                {
                    FactId = Guid.NewGuid(),
                    ContentId = 0,
                    Culture = "en-us",
                    Location = new Location
                    {
                        City = "New York",
                        Province = "New York",
                        Country = "United States of America",
                        Continent = "North America"

                    },
                    UserRights = new UserRightsDto
                    {
                        UserId = 0,
                        PortalId = 0,
                        Name = "Mr Andersson",
                        Email = "no@mail.com",
                        UserUgam = 0L,
                        CustomerId = 0,
                        Subscription = CustomerSubscription.Enterprise,
                        Roles = new List<ZeroRole> { ZeroRole.Candidate },
                        UserGroupIds = new List<int> { 1, 2, 4 }
                    },
                    Timestamp = DateTime.UtcNow,
                    Platform = 0,
                    StatisticStartDate = DateTime.UtcNow.AddMonths(-1)
                });
            }
            await _serviceBus.Send(KnownTopics.Statistics.ArticleViewed, messages);
            return new AcceptedResult();
        }
    }
}
