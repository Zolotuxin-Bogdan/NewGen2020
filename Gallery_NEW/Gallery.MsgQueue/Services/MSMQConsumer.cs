using System;
using System.Messaging;
using Gallery.BLL.Contracts;
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
        }
        public object ReceiveFirstMessageBody()
        {
            SetMessageFormat(new Type[]
            {
                typeof(MessageDTO)
            });

            return _messageQueue.Receive().Body;
        }

        private void SetMessageFormat(Type[] msgTypes)
        {
            _messageQueue.Formatter = new XmlMessageFormatter(msgTypes);
        }
    }
}
