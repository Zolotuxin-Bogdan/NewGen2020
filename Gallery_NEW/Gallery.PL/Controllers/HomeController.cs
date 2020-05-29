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
using Gallery.PL.Manager;

namespace Gallery.Controllers
{
    public class HomeController : Controller
    {
        private IimageService _imageService;
        private IHashService _hashService;
        public HomeController(IimageService imageService, IHashService hashService)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        }

        [Authorize]
        [LogFilter]
        [HttpGet]
        public async Task<ActionResult> Delete(string pathForDelete)
        {
            var mediaPath = Server.MapPath(pathForDelete);
            var isDeleteSuccess = await _imageService.DeleteMediaAsync(mediaPath);

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

            var defaultPath = GalleryConfig.GetPathForSave();
            var directoryPath = Server.MapPath(defaultPath) + _hashService.ComputeSha256Hash(User.Identity.Name);
            var mediaPath = Path.Combine(directoryPath, _hashService.ComputeSha256Hash(data));

            var userId = Convert.ToInt32(User.Identity.Name);
            var isUploadSuccess = await _imageService.UploadImageAsync(data, mediaPath, userId);
            if (!isUploadSuccess)
            {
                ViewBag.Error = "Something happened wrong... Try it again.";
                return View("Error");
            }

            return RedirectToAction("Index");

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }


        public ActionResult Upload()
        {
            return View();
        }

    }
}