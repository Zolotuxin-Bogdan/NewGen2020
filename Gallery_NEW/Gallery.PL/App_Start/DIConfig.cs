using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using FileStorageProvider.Providers;
using FileStorageProvider.Interfaces;
using Gallery.BLL;
using Gallery.DAL.Model;
using Gallery.DAL.Repositories;
using Gallery.DAL.Repositories.Interfaces;
using Gallery.MsgQueue.Azure.Implementation;
using Gallery.MsgQueue.Interfaces;
using Gallery.PL.Interfaces;
using Gallery.PL.Services;
using Gallery.MsgQueue.RabbitMQ.Implementation;
using Gallery.MsgQueue.MSMQ.Implementation;

namespace Gallery.PL.App_Start
{
    public static class DIConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var connectionString = ConfigurationManager.ConnectionStrings["sql"].ConnectionString ?? throw new ArgumentException("sql");

            builder.Register(ctx => new GalleryContext(connectionString))
                .AsSelf();

            builder.RegisterType<UsersRepository>()
                .As<IRepository>();

            builder.RegisterType<UsersService>()
                .As<IUsersService>();

            builder.RegisterType<AuthenticationService>()
                .As<IAuthenticationService>();

            builder.RegisterType<MediaService>()
                .As<IMediaService>();

            //builder.RegisterType<GalleryConfig>()
            //  .AsSelf();

            builder.RegisterType<MediaStorageProvider>()
                .As<IMediaStorageProvider>();

            builder.RegisterType<MediaRepository>()
                .As<IMediaRepository>();

            builder.RegisterType<HashService>()
                .As<IHashService>();

            /////
            // MSMQ registration
            //

            // builder.Register(p => new MSMQPublisher())
            //    .As<IPublisher>();
            /////

            /////
            // RabbitMQ registration
            //

            // builder.Register(p => new RabbitMQPublisher())
            //    .As<IPublisher>();
            /////

            /////
            // AzureMQ registration
            //

            builder.Register(p => new AzurePublisher())
                .As<IPublisher>();
            /////


            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}