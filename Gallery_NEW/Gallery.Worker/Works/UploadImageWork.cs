using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileStorageProvider.Interfaces;
using Gallery.BLL;
using Gallery.BLL.Contracts;
using Gallery.DAL.Repositories.Interfaces;
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
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaStorageProvider _mediaStorage;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly TimeSpan _delayTime = TimeSpan.FromSeconds(1);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UploadImageWork(IConsumer consumer, IMediaService mediaService, IMediaRepository mediaRepository, IMediaStorageProvider mediaStorage)
        {
            _consumer = consumer;
            _mediaService = mediaService;
            _mediaRepository = mediaRepository;
            _mediaStorage = mediaStorage;
        }

        public async Task StartAsync()
        {
            var parseMessageQueueDictionary = MessageQueueParser.ParseMessageQueuePathsDictionary();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var messageDTO = _consumer.ReceiveFirstMessage<MessageDTO>(parseMessageQueueDictionary["upload-image"]);

                if (!File.Exists(messageDTO.TempPath))
                {
                    throw new FileNotFoundException("File is missing:", messageDTO.TempPath);
                }

                var isTempMediaExist =
                    await _mediaRepository.IsTempMediaExistByNameAndLoadingStatusAsync(messageDTO.FileName, true);

                if (isTempMediaExist)
                {
                    var tempMedia =
                        await _mediaRepository.GetTempMediaByNameAndLoadingStatusAsync(messageDTO.FileName, true);

                    var newTempMedia = tempMedia;

                    newTempMedia.IsLoading = false;
                    newTempMedia.IsSuccess = true;

                    var fileBytes = _mediaStorage.Read(messageDTO.TempPath);
                    await _mediaService.UploadImageAsync(fileBytes, messageDTO.MainPath, messageDTO.UserId);

                    await _mediaRepository.ChangeTempMediaAsync(tempMedia, newTempMedia);
                }

                Logger.Info("Started " + nameof(UploadImageWork) + ".");
                await Task.Delay(_delayTime);
            }
        }

        public void Stop()
        {
            Logger.Info("Stopping " + nameof(UploadImageWork) + ".");
            _cancellationTokenSource.Cancel();
            Logger.Info("Stoped " + nameof(UploadImageWork) + ".");
        }
    }
}
