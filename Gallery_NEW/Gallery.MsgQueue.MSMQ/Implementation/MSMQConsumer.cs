using System;
using System.Messaging;
using Gallery.MsgQueue.Interfaces;
using Newtonsoft.Json;

namespace Gallery.MsgQueue.MSMQ.Implementation
{
    public class MSMQConsumer : IConsumer
    {
        public void ConsumeFirstMessage<T>(string messageQueuePath, Action<T> action)
        {
            var messageQueue = new MessageQueue(messageQueuePath)
            {
                Formatter = new XmlMessageFormatter(new Type[]
                {
                    typeof(string)
                })
            };
            var messageDto = DeserializeJson<T>((string)messageQueue.Receive().Body);
            action(messageDto);
        }

        private static T DeserializeJson<T>(string obj) => 
            JsonConvert.DeserializeObject<T>(obj);
    }
}
