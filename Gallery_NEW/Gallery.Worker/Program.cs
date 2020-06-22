using Gallery.MsgQueue.Services;

namespace Gallery.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var parseMessageQueue = MessageQueueParser.ParseMessageQueuePaths();
            MessageQueueCreator.CreateMessageQueues(parseMessageQueue);

            TopShelfConfig.Configure();
        }
    }
}
