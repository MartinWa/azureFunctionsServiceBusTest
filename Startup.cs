using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Test.Settings;

[assembly: FunctionsStartup(typeof(Test.Startup))]
namespace Test
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();

            var loggerFactory = new LoggerFactory();

            builder.Services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBus"));
        }
    }
}