using Gallery.MsgQueue.Services;

namespace Gallery.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var parseMessageQueueDictionary = MessageQueueParser.ParseMessageQueuePathsDictionary();

            MessageQueueCreator.CreateMSMQMessageQueues(parseMessageQueueDictionary);
            MessageQueueCreator.CreateRabbitMQMessageQueues(parseMessageQueueDictionary);

            TopShelfConfig.Configure();
        }
    }
}
