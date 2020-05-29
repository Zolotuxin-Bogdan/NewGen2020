using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gallery.PL.Interfaces
{
    public interface IPublisher
    {
        void SendMessage(object message, string label);
    }
}