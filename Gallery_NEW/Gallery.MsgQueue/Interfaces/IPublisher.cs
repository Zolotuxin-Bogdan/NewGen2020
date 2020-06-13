using System;

namespace Gallery.MsgQueue.Interfaces
{
    public interface IPublisher
    {
        void SendMessage(object message, string label);
        void SetMessageFormat(Type[] msgTypes);
    }
}