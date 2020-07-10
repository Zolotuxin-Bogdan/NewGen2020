using System;
using System.Threading;
using System.Threading.Tasks;
using Gallery.BLL;
using Gallery.BLL.Contracts;
using Gallery.MsgQueue.Interfaces;
using Gallery.MsgQueue.Services;
using Gallery.Worker.Interfaces;
using NLog;

namespace Gallery.Worker.Works
{
    public class UploadImageWork : IWork
    {
        private readonly IConsumer _consumer;
        private readonly IMediaService _mediaService;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly TimeSpan _delayTime = TimeSpan.FromSeconds(1);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UploadImageWork(IConsumer consumer, IMediaService mediaService)
        {
            _consumer = consumer;
            _mediaService = mediaService;
        }

        public async Task StartAsync()
        {
            var parseMessageQueueDictionary = MessageQueueParser.ParseMessageQueuePathsDictionary();

            while (!_cancellationTokenSource.IsCancellationRequested)
            { 
                _consumer.ConsumeFirstMessage<MessageDTO>(
                    parseMessageQueueDictionary["upload-image"],
                    async m => await _mediaService.MoveTempImageToUserDirectoryAsync(m));
            }

            Logger.Info("Started " + nameof(UploadImageWork) + ".");
            await Task.Delay(_delayTime);
        }

        public void Stop()
        {
            Logger.Info("Stopping " + nameof(UploadImageWork) + ".");
            _cancellationTokenSource.Cancel();
            Logger.Info("Stoped " + nameof(UploadImageWork) + ".");
        }
    }
}
