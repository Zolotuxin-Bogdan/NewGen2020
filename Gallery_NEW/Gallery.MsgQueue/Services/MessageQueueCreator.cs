using System.Collections.Generic;
using System.Messaging;

namespace Gallery.MsgQueue.Services
{
    public static class MessageQueueCreator
    {
        public static void CreateMSMQMessageQueues(List<string> msgQueueList)
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
