using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Worker
{
    public static class Factory
    {
        public static ConsumerConfiguration ParseConsumerConfiguration()
        {
            var path = ConfigurationManager.AppSettings["msmq:path"] ?? @".\private$\MQ";

            return new ConsumerConfiguration(
                path: @".\private$\MQ");
        }
    }

    public class ConsumerConfiguration
    {
        public string Path { get; }

        public ConsumerConfiguration(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
