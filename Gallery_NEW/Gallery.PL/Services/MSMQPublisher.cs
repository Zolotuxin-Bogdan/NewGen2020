using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gallery.PL.Interfaces;
using System.Messaging;

namespace Gallery.PL.Services
{
    public class MsmqPublisher : IPublisher
    {
        private readonly MessageQueue _messageQueue;

        public MsmqPublisher(MessageQueue messageQueue)
        {
            _messageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
        }

        public void SendMessage(object message, string label)
        {
            _messageQueue.Send(message, label);
        }
    }
}