using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorageProvider.Providers;
using Gallery.BLL;
using Gallery.DAL.Model;
using Gallery.DAL.Repositories;
using Gallery.MsgQueue.Services;
using Gallery.Worker.Interfaces;
using Gallery.Worker.Works;

namespace Gallery.Worker
{
    public static class Factory
    {
        public static IWork GetUploadImageWork()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sql"] ?? throw new ArgumentException("SQL");
            var parseMessageQueues = MessageQueueParser.ParseMessageQueuePaths();

            var uploadImageWork = new UploadImageWork(new MSMQConsumer(parseMessageQueues[0]), 
                new MediaService(new MediaStorageProvider(), 
                    new MediaRepository(new GalleryContext(connectionString.ConnectionString)), 
                    new UsersRepository(new GalleryContext(connectionString.ConnectionString))), 
                new MediaRepository(new GalleryContext(connectionString.ConnectionString)), new MediaStorageProvider());

            return uploadImageWork;
        }
    }
}
