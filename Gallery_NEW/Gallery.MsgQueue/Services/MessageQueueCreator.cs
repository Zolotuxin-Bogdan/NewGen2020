using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using RabbitMQ.Client;

namespace Gallery.MsgQueue.Services
{
    public static class MessageQueueCreator
    {
        public static void CreateMSMQMessageQueues(List<string> msgQueueList)
        {
            foreach (var queue in msgQueueList)
            {
                if (string.IsNullOrWhiteSpace(queue))
                {
                    continue;
                }

                if (!MessageQueue.Exists(queue))
                {
                    MessageQueue.Create(queue);
                }
            }
        }

        public static void CreateRabbitMQMessageQueues(List<string> msgQueueList)
        {
            var connectionString = new Uri(ParseRabbitMQConnectionString());
            ;
            var factory = new ConnectionFactory() {Uri = connectionString};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    foreach (var queue in msgQueueList)
                    {
                        channel.QueueDeclare(queue: queue,
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
