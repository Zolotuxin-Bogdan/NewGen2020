using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorageProvider.Interfaces
{
    public interface IMediaStorageProvider
    {
        bool Upload(byte[] dataBytes, string path);
        byte[] Read(string path);
        bool Delete(string path);
    }
}
