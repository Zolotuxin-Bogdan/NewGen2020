using FileStorageProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;
using Gallery.BLL.Contracts;
using Gallery.DAL.Model;
using Gallery.DAL.Repositories.Interfaces;

namespace Gallery.BLL
{
    public class MediaService : IMediaService
    {
        private readonly IMediaStorageProvider _mediaStorage;
        private readonly IMediaRepository _mediaRepository;
        private readonly IRepository _userRepository;

        public MediaService(IMediaStorageProvider mediaStorage, 
            IMediaRepository mediaRepository, 
            IRepository userRepository)
        {
            _mediaStorage = mediaStorage ?? throw new ArgumentNullException(nameof(mediaStorage));
            _mediaRepository = mediaRepository ?? throw new ArgumentNullException(nameof(mediaRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }



        //
        // check for equality of pictures
        // Input: Bitmap1, Bitmap2
        // Output:
        //        true - is equal
        //        false - isn't equal
        //
        public bool CompareBitmapsAsync(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
        }

        public async Task<bool> UploadImageAsync(byte[] data, string path, int userId)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var isMediaExist = await _mediaRepository.IsMediaExistAsync(path);
            if (isMediaExist)
            {
                var media = await _mediaRepository.GetMediaByNameAsync(path);

                if (media.IsDeleted)
                {
                    await _mediaRepository.ChangeDeleteStatusAsync(path, false);
                }
            }
            else
            {
                var mediaExtension = Path.GetExtension(path);
                var isMediaTypeExist = await _mediaRepository.IsMediaTypeExistAsync(mediaExtension);
                if (!isMediaTypeExist)
                {
                    await _mediaRepository.RegisterMediaTypeToDataBaseAsync(new MediaType {Type = mediaExtension});
                }

                var mediaType = await _mediaRepository.GetMediaTypeByTypeAsync(mediaExtension);
                var user = await _userRepository.GetUserByIdAsync(userId);
                await _mediaRepository.RegisterMediaToDataBaseAsync(new Media
                {
                    Name = path,

                    IsDeleted = false,

                    UserId = userId,
                    
                    MediaTypeId = mediaType.Id
                });
            }

            return _mediaStorage.Upload(data, path);
        }

        public async Task<bool> UploadTempImageAsync(byte[] data, string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            //
            // Add record to UploadAttemptMedia
            //

           return _mediaStorage.Upload(data, path);
        }

        public async Task<bool> DeleteMediaAsync(string path)
        {
            var directoryName = Path.GetDirectoryName(path);

            if (!Directory.Exists(directoryName))
            {
                throw new DirectoryNotFoundException(nameof(directoryName));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(nameof(path));
            }

            var isMediaExist = await _mediaRepository.IsMediaExistAsync(path);

            if (isMediaExist)
            {
                var media = await _mediaRepository.GetMediaByNameAsync(path);
                if (!media.IsDeleted)
                {
                    await _mediaRepository.ChangeDeleteStatusAsync(path, true);
                }
            }

            return _mediaStorage.Delete(path);
        }

        public ExifDTO GetExifData(string tempPath)
        {
            var tempDirectoryName = Path.GetDirectoryName(tempPath);

            if (!Directory.Exists(tempDirectoryName))
            {
                throw new DirectoryNotFoundException(nameof(tempDirectoryName));
            }

            var exifDto = new ExifDTO();

            using (FileStream fileStream = new FileStream(tempPath, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(tempPath);
                BitmapSource img = BitmapFrame.Create(fileStream);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;

                //
                // Title from EXIF
                //
                if (string.IsNullOrEmpty(md.Title))
                {
                    exifDto.Title = "Data not found";
                }
                else
                {
                    exifDto.Title = md.Title;
                }

                //
                // Manufacturer from EXIF
                //
                if (string.IsNullOrEmpty(md.CameraManufacturer))
                {
                    exifDto.Manufacturer = "Data not found";
                }
                else
                {
                    exifDto.Manufacturer = md.CameraManufacturer;
                }

                //
                // ModelOfCamera from EXIF
                //
                if (string.IsNullOrEmpty(md.CameraModel))
                {
                    exifDto.ModelOfCamera = "Data not found";
                }
                else
                {
                    exifDto.ModelOfCamera = md.CameraModel;
                }

                //
                // DateCreation from EXIF
                //
                if (string.IsNullOrEmpty(md.DateTaken))
                {
                    exifDto.DateCreation = "Data not found";
                }
                else
                {
                    exifDto.DateCreation = md.DateTaken;
                }

                //
                // FileSize from FileInfo
                //
                if (fileInfo.Length >= 1024)
                {
                    exifDto.FileSize = Math.Round((fileInfo.Length / 1024f), 1).ToString() + " KB";
                    if ((fileInfo.Length / 1024f) >= 1024f)
                    {
                        exifDto.FileSize = Math.Round((fileInfo.Length / 1024f) / 1024f, 2).ToString() + " MB";
                    }
                }
                else
                {
                    exifDto.FileSize = fileInfo.Length.ToString() + " B";
                }

                //
                // DateUpload from FileInfo
                //
                if (string.IsNullOrEmpty(fileInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss")))
                {
                    exifDto.DateUpload = "Data not found";
                }
                else
                {
                    exifDto.DateUpload = fileInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss");
                }
            }

            return exifDto;
        }


    }
}
