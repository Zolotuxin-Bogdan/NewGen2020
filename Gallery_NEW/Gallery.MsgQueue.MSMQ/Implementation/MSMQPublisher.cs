using System;
using System.Messaging;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;

namespace Gallery.MsgQueue.MSMQ.Implementation
{
    public class MSMQPublisher : IPublisher
    {
        public void SendMessage<T>(T message, string messageQueuePath)
        {
            var messageQueue = new MessageQueue(messageQueuePath)
            {
                Formatter = new XmlMessageFormatter(new Type[]
                {
                    typeof(string)
                })
            };
            var messageJson = SerializeToJson(message);
            messageQueue.Send(messageJson);
        }

        private static string SerializeToJson<T>(T obj) =>
            JsonConvert.SerializeObject(obj);
    }
}