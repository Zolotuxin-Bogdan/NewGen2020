using Gallery.MsgQueue.Services;

namespace Gallery.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var parseMsmqMessageQueue = MessageQueueParser.ParseMsmqMessageQueuePaths();
            MessageQueueCreator.CreateMSMQMessageQueues(parseMsmqMessageQueue);

            TopShelfConfig.Configure();
        }
    }
}
