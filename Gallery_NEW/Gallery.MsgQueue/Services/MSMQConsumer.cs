using System;
using System.Messaging;
using Gallery.MsgQueue.Interfaces;

namespace Gallery.MsgQueue.Services
{
    public class MSMQConsumer : IConsumer
    {
        private readonly MessageQueue _messageQueue;
        private readonly string _messageQueuePath;

        public MSMQConsumer(string messageQueuePath)
        {
            _messageQueuePath = messageQueuePath ?? throw new ArgumentNullException(nameof(messageQueuePath));
            _messageQueue = new MessageQueue(_messageQueuePath);

            if (!MessageQueue.Exists(_messageQueue.Path))
            {
                MessageQueue.Create(_messageQueue.Path);
            }

        }
        public object ReceiveFirstMessageBody()
        {
            return _messageQueue.Receive().Body;
        }

        public void SetMessageFormat(Type[] msgTypes)
        {
            _messageQueue.Formatter = new XmlMessageFormatter(msgTypes);
        }
    }
}
