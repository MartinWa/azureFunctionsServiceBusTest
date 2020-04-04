using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Test.AzureServiceBus.Messages
{
    public static class MessageExtensions
    {
        public static TType BodyAs<TType>(this Message self)
        {
            var json = Encoding.UTF8.GetString(self.Body);
            return JsonConvert.DeserializeObject<TType>(json);
        }
    }
}
