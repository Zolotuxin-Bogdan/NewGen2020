using Gallery.MsgQueue.Interfaces;
using System;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Gallery.MsgQueue.RabbitMQ.Implementation
{
    public class RabbitMQPublisher : IPublisher
    {
        public void SendMessage<T>(T message, string messageQueuePath)
        {
            var connectionString = new Uri(ParseRabbitMQConnectionString());;
            var factory = new ConnectionFactory() { Uri = connectionString };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var messageBody = SerializeToBytes(SerializeToJson(message));

                    channel.BasicPublish(exchange: "",
                        routingKey: messageQueuePath,
                        basicProperties: null,
                        body: messageBody);
                }
            }
        }

        private static string ParseRabbitMQConnectionString()
        {
            var connectionString = ConfigurationManager.AppSettings["RabbitMQ:uri"] ?? throw new ArgumentException("RabbitMQ:uri");
            return connectionString;
        }

        private static string SerializeToJson<T>(T obj) =>
            JsonConvert.SerializeObject(obj);

        private static byte[] SerializeToBytes(string obj) =>
            Encoding.UTF8.GetBytes(obj);
    }
}
