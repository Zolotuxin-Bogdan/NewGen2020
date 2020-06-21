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
        public static List<string> ParseMessageQueuePaths()
        { 
            var messageQueues = ConfigurationManager.AppSettings["msgQueue:paths"] ?? throw new ArgumentException("msgQueue:paths");

            return messageQueues.Split(',').ToList();
        }
    }
}
