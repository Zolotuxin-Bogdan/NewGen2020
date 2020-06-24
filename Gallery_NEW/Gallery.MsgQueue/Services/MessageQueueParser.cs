using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.MsgQueue.Services
{
    public static class MessageQueueParser
    {
        public static List<string> ParseMsmqMessageQueuePaths()
        { 
            var messageQueues = ConfigurationManager.AppSettings["MSMQ:paths"] ?? throw new ArgumentException("MSMQ:paths");

            return messageQueues.Split(',').ToList();
        }

        public static List<string> ParseRabbitMQMessageQueuePaths()
        {
            var messageQueues = ConfigurationManager.AppSettings["RabbitMQ:paths"] ?? throw new ArgumentException("RabbitMQ:paths");

            return messageQueues.Split(',').ToList();
        }
    }
}
