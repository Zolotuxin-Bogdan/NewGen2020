using System;
using System.Messaging;
using Gallery.BLL.Contracts;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;

namespace Gallery.MsgQueue.Services
{
    public class MSMQConsumer : IConsumer
    {
        public T ReceiveFirstMessageBody<T>(string messageQueuePath)
        {
            var messageQueue = new MessageQueue(messageQueuePath)
            {
                Formatter = new XmlMessageFormatter(new Type[]
                {
                    typeof(string)
                })
            };

            return DeserializeJson<T>((string)messageQueue.Receive().Body);
        }

        private static T DeserializeJson<T>(string obj) => 
            JsonConvert.DeserializeObject<T>(obj);
    }
}
