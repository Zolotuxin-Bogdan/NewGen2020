using System;
using System.Configuration;
using FileStorageProvider.Providers;
using Gallery.BLL;
using Gallery.DAL.Model;
using Gallery.DAL.Repositories;
using Gallery.MsgQueue.RabbitMQ.Implementation;
using Gallery.Worker.Interfaces;
using Gallery.Worker.Works;

namespace Gallery.Worker
{
    public static class Factory
    {
        public static IWork GetUploadImageWork()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sql"] ?? throw new ArgumentException("SQL");

            var uploadImageWork = new UploadImageWork(new RabbitMQConsumer(), 
                new MediaService(new MediaStorageProvider(), 
                    new MediaRepository(new GalleryContext(connectionString.ConnectionString)), 
                    new UsersRepository(new GalleryContext(connectionString.ConnectionString))));

            return uploadImageWork;
        }
    }
}
