using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorageProvider.Interfaces;

namespace FileStorageProvider.Providers
{
    public class MediaStorageProvider : IMediaStorageProvider
    {
        public bool Upload(byte[] dataBytes, string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (dataBytes == null)
            {
                throw new ArgumentNullException(nameof(dataBytes));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Argument_EmptyPath", nameof(path));
            }

            File.WriteAllBytes(path, dataBytes);
            return File.Exists(path);
        }
        public byte[] Read(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Argument_EmptyPath", nameof(path));
            }

            return File.ReadAllBytes(path);
        }
        public bool Delete(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Argument_EmptyPath", nameof(path));
            }

            File.Delete(path);
            return !File.Exists(path);
        }


    }
}
