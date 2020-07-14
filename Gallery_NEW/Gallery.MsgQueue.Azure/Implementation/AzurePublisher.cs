using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;

namespace Gallery.MsgQueue.Azure.Implementation
{
    public class AzurePublisher : IPublisher
    {
        public void SendMessage<T>(T message, string messageQueuePath)
        {
            var connectionString = ParseAzureMQConnectionString();

            var queueServiceClient = new QueueServiceClient(connectionString);

            var queueClient = queueServiceClient.GetQueueClient(messageQueuePath);

            var messageJson = SerializeToJson(message);

            queueClient.SendMessage(messageJson);
        }

        private static string ParseAzureMQConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureMQ:connectionString"].ConnectionString
                                   ?? throw new ArgumentException("AzureMQ:connectionString");
            return connectionString;
        }

        private static string SerializeToJson<T>(T obj) =>
            JsonConvert.SerializeObject(obj);
    }
}
