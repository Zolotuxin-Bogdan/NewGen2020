using System;
using System.Configuration;
using System.Threading;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;

namespace Gallery.MsgQueue.Azure.Implementation
{
    public class AzureConsumer : IConsumer
    {
        private readonly TimeSpan _delayTime = TimeSpan.FromSeconds(1);

        public void ConsumeFirstMessage<T>(string messageQueuePath, Action<T> action)
        {
            var connectionString = ParseAzureMQConnectionString();

            var queueServiceClient = new QueueServiceClient(connectionString);
            var queueClient = queueServiceClient.GetQueueClient(messageQueuePath);

            while (true)
            {
                QueueProperties queueProperties = queueClient.GetProperties();

                var msgCount = queueProperties.ApproximateMessagesCount;
                if (msgCount > 0)
                {
                    QueueMessage[] receiveMessages = queueClient.ReceiveMessages(maxMessages: 1);

                    var message = DeserializeJson<T>(receiveMessages[0].MessageText);

                    queueClient.DeleteMessage(receiveMessages[0].MessageId, receiveMessages[0].PopReceipt);

                    action(message);
                    break;
                }
                Thread.Sleep(_delayTime);
            }
        }

        private static string ParseAzureMQConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureMQ:connectionString"].ConnectionString
                                   ?? throw new ArgumentException("AzureMQ:connectionString");
            return connectionString;
        }

        private static T DeserializeJson<T>(string obj) =>
            JsonConvert.DeserializeObject<T>(obj);
    }
}
