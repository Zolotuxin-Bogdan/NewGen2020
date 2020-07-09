using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gallery.BLL;
using System.Threading.Tasks;
using Gallery.BLL.Contracts;
using Gallery.PL.Filters;
using Gallery.PL.Manager;
using Gallery.MsgQueue.Interfaces;
using Gallery.MsgQueue.Services;

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
            var uniqFileName = _hashService.ComputeSha256Hash(data);

            var defaultPath = GalleryConfig.GetPathForSave();
            var defaultTempPath = GalleryConfig.GetTempPath();

            var directoryPath = Server.MapPath(defaultPath) + _hashService.ComputeSha256Hash(User.Identity.Name);
            var directoryTempPath = Server.MapPath(defaultTempPath);

            var mediaPath = Path.Combine(directoryPath, _hashService.ComputeSha256Hash(data) + mediaExtension);
            var mediaTempPath = Path.Combine(directoryTempPath, _hashService.ComputeSha256Hash(data) + mediaExtension);

            var userId = Convert.ToInt32(User.Identity.Name);

            var tempMediaDTO = new TempMediaDTO
            {
                UniqName = uniqFileName,
                IsLoading = true,
                IsSuccess = false,
                UserId = userId
            };

            var isTempUploadSuccess = await _mediaService.UploadTempImageAsync(data, mediaTempPath, tempMediaDTO);
            if (!isTempUploadSuccess)
            {
                ViewBag.Error = "Something happened wrong... Try it again.";
                return View("Error");
            }

            var messageDTO = new MessageDTO
            {
                UserId = userId,
                FileName = uniqFileName,
                MainPath = mediaPath,
                TempPath = mediaTempPath
            };

            var parseMessageQueueDictionary = MessageQueueParser.ParseMessageQueuePathsDictionary();

            _publisher.SendMessage(messageDTO, parseMessageQueueDictionary["upload-image"]);

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