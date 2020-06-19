using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileStorageProvider.Interfaces;
using Gallery.BLL;
using Gallery.BLL.Contracts;
using Gallery.DAL.Model;
using Gallery.DAL.Repositories.Interfaces;
using Gallery.MsgQueue.Interfaces;
using Gallery.Worker.Interfaces;

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

        public UploadImageWork(IConsumer consumer, IMediaService mediaService, IMediaRepository mediaRepository, IMediaStorageProvider mediaStorage)
        {
            _consumer = consumer;
            _mediaService = mediaService;
            _mediaRepository = mediaRepository;
            _mediaStorage = mediaStorage;
        }

        public async Task StartAsync()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var messageBody = _consumer.ReceiveFirstMessageBody();

                var messageDTO = (messageBody as MessageDTO) ?? throw new ArgumentNullException(nameof(messageBody));

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

                await Task.Delay(_delayTime);
            }
        }

        public void Stop()
        { 
            _cancellationTokenSource.Cancel();
        }
    }
}
