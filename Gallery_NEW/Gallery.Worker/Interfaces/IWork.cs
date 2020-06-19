using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Worker.Interfaces
{
    public interface IWork
    {
        Task StartAsync();

        void Stop();
    }
}
