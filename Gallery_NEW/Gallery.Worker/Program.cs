using Gallery.MsgQueue.Services;

namespace Gallery.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var parseMsmqMessageQueue = MessageQueueParser.ParseMsmqMessageQueuePaths();
            MessageQueueCreator.CreateMessageQueues(parseMsmqMessageQueue);

            TopShelfConfig.Configure();
        }
    }
}
