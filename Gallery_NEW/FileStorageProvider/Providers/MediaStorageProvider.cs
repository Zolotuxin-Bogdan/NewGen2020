using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorageProvider.Interfaces;

namespace FileStorageProvider.Providers
{
    public class MediaStorageProvider : IMediaStorageProvider
    {
        public bool Delete(string path)
        {
            throw new NotImplementedException();
        }

        public byte[] Read(string path)
        {
            throw new NotImplementedException();
        }

        public bool Upload(byte[] dataBytes, string path)
        {
            throw new NotImplementedException();
        }
    }
}
