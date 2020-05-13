using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Test.Functions
{
    public class Status
    {
        private readonly ILogger _log;

        public Status(ILogger logger)
        {
            _log = logger;
        }

        [FunctionName("Status")]
        public IActionResult RunStatus([HttpTrigger(AuthorizationLevel.Function, "get", Route = "status")]HttpRequest req)
        {
            var now = DateTime.Now;
            try
            {
                _log.LogInformation($"Running status httpTrigger at {now}");
                return new JsonResult(new { value = $"all is well at {now}" });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"{ex.Message}");
            }
            return new JsonResult(new { value = $"something is wrong at {now}" });
        }
    }
}