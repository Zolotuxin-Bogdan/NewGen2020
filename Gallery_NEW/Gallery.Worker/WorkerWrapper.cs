using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gallery.Worker.Interfaces;

namespace Gallery.Worker
{
    public class WorkerWrapper
    {
        private readonly IReadOnlyCollection<IWork> _works;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public WorkerWrapper(params IWork[] works)
        {
            _works = works ?? throw new ArgumentNullException(nameof(works));
        }

        public async Task StartAsync()
        {
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
            _cancellationTokenSource.Cancel();
        }
    }
}
