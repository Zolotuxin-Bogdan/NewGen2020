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
using Gallery.DAL.Model;
using Gallery.DAL.Repositories.Interfaces;

namespace Gallery.BLL
{
    public class ImageService : IimageService
    {
        private readonly IMediaStorageProvider _mediaStorage;
        private readonly IMediaRepository _mediaRepository;
        private readonly IRepository _userRepository;

        public ImageService(IMediaStorageProvider mediaStorage, 
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

                    UserId = userId,
                    User = user,

                    MediaTypeId = mediaType.Id,
                    Type = mediaType
                });
            }

            return _mediaStorage.Upload(data, path);
        }

        /*public static void LoadExifData(byte[] data)
        {
            string title;
            string manufacturer;
            string modelOfCamera;
            string fileSize;
            string dateCreation;
            string dateUpload;
            manufacturer = "Data not found";
            modelOfCamera = "Data not found";
            fileSize = "Data not found";
            dateCreation = "Data not found";
            dateUpload = "Data not found";


            Stream stream = new MemoryStream(data);
            FileInfo fileInfo = new FileInfo(stream);

            //FileInfo fileInfo = new FileInfo(LoadExifPath);
            //FileStream ExifFileStream = new FileStream(LoadExifPath, FileMode.Open);
            try
            {

                BitmapSource img = BitmapFrame.Create(stream);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;

                //
                //FileSize from FileInfo
                if (fileInfo.Length >= 1024)
                {
                    fileSize = Math.Round((fileInfo.Length / 1024f), 1).ToString() + " KB";
                    if ((fileInfo.Length / 1024f) >= 1024f)
                    {
                        fileSize = Math.Round((fileInfo.Length / 1024f) / 1024f, 2).ToString() + " MB";
                    }
                }
                else
                {
                    fileSize = fileInfo.Length.ToString() + " B";
                }

                //
                //DateUpload from FileInfo
                if (fileInfo.CreationTime == null)
                {
                    dateUpload = "Data not found";
                }
                else
                {
                    dateUpload = fileInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss");
                }

                //
                //title from FileInfo
                if (string.IsNullOrEmpty(fileInfo.Name))
                {
                    title = "Data not found";
                }
                else
                {
                    title = fileInfo.Name;
                }


                //
                //manufacturer from EXIF
                if (string.IsNullOrEmpty(md.CameraManufacturer))
                {
                    manufacturer = "Data not found";
                }
                else
                {
                    manufacturer = md.CameraManufacturer;
                }


                //
                //modelOfCamera from EXIF
                if (string.IsNullOrEmpty(md.CameraModel))
                {
                    modelOfCamera = "Data not found";
                }
                else
                {
                    modelOfCamera = md.CameraModel;
                }

                //
                //DateCreation from EXIF
                if (string.IsNullOrEmpty(md.DateTaken))
                {
                    dateCreation = "Data not found";
                }
                else
                {
                    dateCreation = md.DateTaken;
                }
            }
            catch (Exception err)
            {

                // need to static errors
            }
            ExifFileStream.Close();

        }*/


    }
}
