using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Azure.Storage.Queues;
using RabbitMQ.Client;

namespace Gallery.MsgQueue.Services
{
    public static class MessageQueueCreator
    {
        public static void CreateMSMQMessageQueues(Dictionary<string, string> msgQueueDictionary)
        {
            foreach (var queue in msgQueueDictionary)
            {
                if (string.IsNullOrWhiteSpace(queue.Value))
                {
                    continue;
                }

                var MSMQueue = ".\\private$\\" + queue.Value;

                if (!MessageQueue.Exists(MSMQueue))
                {
                    MessageQueue.Create(MSMQueue);
                }
            }
        }

        public static void CreateRabbitMQMessageQueues(Dictionary<string, string> msgQueueDictionary)
        {
            var connectionString = new Uri(ParseRabbitMQConnectionString());
            var factory = new ConnectionFactory() {Uri = connectionString};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    foreach (var queue in msgQueueDictionary)
                    {
                        channel.QueueDeclare(queue: queue.Value,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
                    }
                }
            }
        }

        public static void CreateAzureMessageQueues(Dictionary<string, string> msgQueueDictionary)
        {
            var connectionString = ParseAzureMQConnectionString();
            var queueServiceClient = new QueueServiceClient(connectionString);
            foreach (var queue in msgQueueDictionary)
            {
                var queueClient = queueServiceClient.GetQueueClient(queue.Value);
                queueClient.CreateIfNotExists();
            }
        }

        private static string ParseRabbitMQConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMQ:connectionString"].ConnectionString
                                   ?? throw new ArgumentException("RabbitMQ:connectionString");
            return connectionString;
        }

        private static string ParseAzureMQConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureMQ:connectionString"].ConnectionString
                                   ?? throw new ArgumentException("AzureMQ:connectionString");
            return connectionString;
        }
    }
}
