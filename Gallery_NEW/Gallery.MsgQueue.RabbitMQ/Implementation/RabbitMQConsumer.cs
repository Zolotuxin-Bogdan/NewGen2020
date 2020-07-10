using System;
using System.Configuration;
using System.Text;
using System.Threading;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Gallery.MsgQueue.RabbitMQ.Implementation
{
    public class RabbitMQConsumer : IConsumer
    {
        private readonly TimeSpan _delayTime = TimeSpan.FromSeconds(1);
        public void ConsumeFirstMessage<T>(string messageQueuePath, Action<T> action)
        {
            var connectionString = new Uri(ParseRabbitMQConnectionString());
            var factory = new ConnectionFactory() {Uri = connectionString };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    while (true)
                    {
                        var msgCount = channel.MessageCount(messageQueuePath);
                        if (msgCount > 0)
                        {
                            var result = channel.BasicGet(messageQueuePath, true);
                            var messageBody = result.Body.ToArray();
                            var messageDto = DeserializeJson<T>(DeserializeBytes(messageBody));
                            action(messageDto);
                            break;
                        }

                        Thread.Sleep(_delayTime);
                    }
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

