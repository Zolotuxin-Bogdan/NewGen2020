using System;

namespace Gallery.MsgQueue.Interfaces
{
    public interface IPublisher
    {
        void SendMessage<T>(T message, string messageQueuePath);
    }
}