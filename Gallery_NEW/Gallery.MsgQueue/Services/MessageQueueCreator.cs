using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.MsgQueue.Services
{
    public static class MessageQueueCreator
    {
        public static void CreateMessageQueues(List<string> msgQueueList)
        {
            foreach (var msg in msgQueueList)
            {
                if (string.IsNullOrWhiteSpace(msg))
                {
                    continue;
                }

                if (!MessageQueue.Exists(msg))
                {
                    MessageQueue.Create(msg);
                }
            }
        }
    }
}
