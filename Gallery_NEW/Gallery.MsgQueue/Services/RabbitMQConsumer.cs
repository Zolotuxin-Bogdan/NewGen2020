using System;
using System.Configuration;
using System.Text;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gallery.MsgQueue.Services
{
    public class RabbitMQConsumer : IConsumer
    {
        public T ReceiveFirstMessage<T>(string messageQueuePath)
        {
            var connectionString = new Uri(ParseRabbitMQConnectionString());
            var factory = new ConnectionFactory() {Uri = connectionString };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var result = channel.BasicGet(messageQueuePath, true);
                    var messageBody = result.Body.ToArray();
                    return DeserializeJson<T>(DeserializeBytes(messageBody));
                }
            }
        }

        private static string DeserializeBytes(byte[] obj) =>
            Encoding.UTF8.GetString(obj);

        private static T DeserializeJson<T>(string obj) =>
            JsonConvert.DeserializeObject<T>(obj);

        private static string ParseRabbitMQConnectionString()
        {
            var connectionString = ConfigurationManager.AppSettings["RabbitMQ:uri"] ??
                                   throw new ArgumentException("RabbitMQ:uri");
            return connectionString;
        }
    }
}

