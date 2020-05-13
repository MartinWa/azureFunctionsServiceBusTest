using System.Reflection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Test;
using Test.AzureServiceBus;
using Test.Settings;

[assembly: FunctionsStartup(typeof(Startup))]
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

            builder.Services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBus"));
            builder.Services.AddSingleton<IServiceBusProxy, ServiceBusProxy>();   
            builder.Services.AddSingleton<ILogger>(x => x.GetService<ILogger<Startup>>());
        }
    }
}