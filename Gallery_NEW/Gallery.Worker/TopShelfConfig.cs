using System;
using Topshelf;

namespace Gallery.Worker
{
    internal static class TopShelfConfig
    {
        internal static void Configure()
        {
            var uploadImageWork = Factory.GetUploadImageWork();

            var exitCode = HostFactory.Run(configure =>
            {
                configure.Service<WorkerWrapper>(service =>
                {
                    service.ConstructUsing(s => new WorkerWrapper(uploadImageWork));
                    service.WhenStarted(async s => await s.StartAsync());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();

                configure.SetDescription("Service for Gallery Worker");
                configure.SetDisplayName("Worker Wrapper Service");
                configure.SetServiceName("WorkerWrapperService");
            });

            var exitCodeValue = (int) Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
