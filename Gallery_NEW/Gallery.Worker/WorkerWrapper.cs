using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gallery.Worker.Interfaces;
using NLog;

namespace Gallery.Worker
{
    public class WorkerWrapper
    {
        private readonly IReadOnlyCollection<IWork> _works;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public WorkerWrapper(params IWork[] works)
        {
            _works = works ?? throw new ArgumentNullException(nameof(works));
        }

        public async Task StartAsync()
        {
            Logger.Info("All works are starting.");
            foreach (var work in _works)
            {
                await Task.Factory.StartNew(work.StartAsync,
                    _cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Current);
            }
        }

        public void Stop()
        {
            Logger.Info("All works are stopping.");
            _cancellationTokenSource.Cancel();

            /*foreach (var work in _works)
            {
                Task.Factory.StartNew(work.Stop,
                   _cancellationTokenSource.Token,
                   TaskCreationOptions.LongRunning,
                   TaskScheduler.Current);
            }*/

            Logger.Info("All works stopped.");
        }
    }
}
