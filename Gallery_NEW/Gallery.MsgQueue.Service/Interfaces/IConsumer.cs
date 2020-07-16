using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.MsgQueue.Interfaces
{
    public interface IConsumer
    {
        void ConsumeFirstMessage<T>(string messageQueuePath, Action<T> action);
    }
}
