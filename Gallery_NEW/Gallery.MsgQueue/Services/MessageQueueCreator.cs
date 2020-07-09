using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
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

        private static string ParseRabbitMQConnectionString()
        {
            var connectionString = ConfigurationManager.AppSettings["RabbitMQ:uri"] ??
                                   throw new ArgumentException("RabbitMQ:uri");
            return connectionString;
        }

    }
}
