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
        public static Dictionary<string, string> ParseMessageQueuePathsDictionary()
        {
            var upload_Image = ConfigurationManager.AppSettings["MQ:upload-image"] ?? throw new ArgumentException("MQ:upload-image");
            var upload_Mp3 = ConfigurationManager.AppSettings["MQ:upload-mp3"] ?? throw new ArgumentException("MQ:upload-mp3");
            var upload_Mp4 = ConfigurationManager.AppSettings["MQ:upload-mp4"] ?? throw new ArgumentException("MQ:upload-mp4");

            Dictionary<string, string> msgQueuePathsDictionary = new Dictionary<string, string>
            {
                ["upload-image"] = upload_Image,
                ["upload-mp3"] = upload_Mp3,
                ["upload-mp4"] = upload_Mp4
            };

            return msgQueuePathsDictionary;
        }
    }
}
