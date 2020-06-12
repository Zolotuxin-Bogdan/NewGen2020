using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Configuration;
using System.Web.Mvc;
using Gallery.BLL;
using System.Threading.Tasks;
using Gallery.PL.Filters;
using Gallery.PL.Interfaces;
using Gallery.PL.Manager;

namespace Gallery.Controllers
{
    public class HomeController : Controller
    {
        private IMediaService _mediaService;
        private IHashService _hashService;
        private IPublisher _publisher;
        private IUsersService _usersService;
        public HomeController(IMediaService mediaService, IHashService hashService, IPublisher publisher, IUsersService usersService)
        {
            _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [Authorize]
        [LogFilter]
        [HttpPost]
        public async Task<ActionResult> Delete(string pathForDelete)
        {
            var mediaPath = Server.MapPath(pathForDelete);
            var isDeleteSuccess = await _mediaService.DeleteMediaAsync(mediaPath);

            if (!isDeleteSuccess)
            {
                ViewBag.Error = "Something happened wrong... Try it again.";
                return View("Error");
            }

            return RedirectToAction("Index");

        }

        [Authorize]
        [LogFilter]
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase files)
        {
            byte[] data;

            using (Stream inputStream = files.InputStream) 
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();

                memoryStream.Close();
            }

            var mediaExtension = Path.GetExtension(files.FileName);

            var defaultPath = GalleryConfig.GetPathForSave();
            var defaultTempPath = GalleryConfig.GetTempPath();

            var directoryPath = Server.MapPath(defaultPath) + _hashService.ComputeSha256Hash(User.Identity.Name);

            var mediaPath = Path.Combine(directoryPath, _hashService.ComputeSha256Hash(data) + mediaExtension);
            var mediaTempPath = Path.Combine(defaultTempPath, _hashService.ComputeSha256Hash(data) + mediaExtension);

            await _mediaService.UploadTempImageAsync(data, mediaTempPath);

            var userId = Convert.ToInt32(User.Identity.Name);
            var isUploadSuccess = await _mediaService.UploadImageAsync(data, mediaPath, userId);
            if (!isUploadSuccess)
            {
                ViewBag.Error = "Something happened wrong... Try it again.";
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index()
        {
            var defaultPath = GalleryConfig.GetPathForSave();
            var defaultTempPath = GalleryConfig.GetTempPath();

            // Directory path with all User's directories
            var fullDefaultPath = Server.MapPath(defaultPath);

            // Directory path with temporary media
            var tempImageDirectory = Server.MapPath(defaultTempPath);

            if (!Directory.Exists(fullDefaultPath))
            {
                Directory.CreateDirectory(fullDefaultPath);
            }

            if (!Directory.Exists(tempImageDirectory))
            {
                Directory.CreateDirectory(tempImageDirectory);
            }

            if (Request.IsAuthenticated)
            {
                // Encrypted User's directory path
                string userDirectoryPath = fullDefaultPath + _hashService.ComputeSha256Hash(User.Identity.Name);

                if (!Directory.Exists(userDirectoryPath))
                {
                    Directory.CreateDirectory(userDirectoryPath);
                }

                var userId = Convert.ToInt32(User.Identity.Name);
                var userEmail = _usersService.GetUserName(userId);

                ViewData["UserEmail"] = userEmail;
                ViewData["UserHashName"] = _hashService.ComputeSha256Hash(User.Identity.Name);
            }

            // Directory path with all User's media
            var imageDirectories = Directory.GetDirectories(fullDefaultPath);

            ViewData["Exif"] = (from dir in imageDirectories
                from file in Directory.GetFiles(dir)
                select _mediaService.GetExifData(file)).ToList();

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var userEmail = _usersService.GetUserName(userId);

            ViewData["UserEmail"] = userEmail;

            return View();
        }

    }
}